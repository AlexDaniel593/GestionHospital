using CapaDatos;
using CapaEntidad;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionHospital.Tests.Unit
{
    /// <summary>
    /// Pruebas unitarias para SecurityDAL
    /// Cobertura: Login attempts, account lockout, failed attempt recording
    /// </summary>
    public class SecurityDALTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly SecurityDAL _securityDAL;

        public SecurityDALTests()
        {
            // Configurar base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _securityDAL = new SecurityDAL(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region LogLoginAttemptAsync Tests

        [Fact]
        public async Task LogLoginAttemptAsync_SuccessfulLogin_SavesAttemptToDatabase()
        {
            // Arrange
            var email = "test@example.com";
            var ipAddress = "192.168.1.1";
            var userAgent = "Mozilla/5.0";

            // Act
            await _securityDAL.LogLoginAttemptAsync(email, true, ipAddress, userAgent);

            // Assert
            var attempts = await _context.LOGIN_ATTEMPTS.ToListAsync();
            attempts.Should().HaveCount(1);
            attempts[0].Email.Should().Be(email);
            attempts[0].IsSuccessful.Should().BeTrue();
            attempts[0].IpAddress.Should().Be(ipAddress);
            attempts[0].UserAgent.Should().Be(userAgent);
        }

        [Fact]
        public async Task LogLoginAttemptAsync_FailedLogin_SavesAttemptWithReason()
        {
            // Arrange
            var email = "test@example.com";
            var failureReason = "Contraseña incorrecta";

            // Act
            await _securityDAL.LogLoginAttemptAsync(email, false, failureReason: failureReason);

            // Assert
            var attempts = await _context.LOGIN_ATTEMPTS.ToListAsync();
            attempts.Should().HaveCount(1);
            attempts[0].IsSuccessful.Should().BeFalse();
            attempts[0].FailureReason.Should().Be(failureReason);
        }

        #endregion

        #region RecordFailedAttemptAsync Tests

        [Fact]
        public async Task RecordFailedAttemptAsync_FirstAttempt_IncrementsCounter()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = await _securityDAL.RecordFailedAttemptAsync(email);

            // Assert
            result.IsLocked.Should().BeFalse();
            var lockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            lockout.FailedAttempts.Should().Be(1);
        }

        [Fact]
        public async Task RecordFailedAttemptAsync_ThirdAttempt_LocksAccount()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);
            var result = await _securityDAL.RecordFailedAttemptAsync(email);

            // Assert
            result.IsLocked.Should().BeTrue();
            result.LockoutEnd.Should().NotBeNull();
            result.LockoutEnd.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(30), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task RecordFailedAttemptAsync_AfterLockout_ResetsCounter()
        {
            // Arrange
            var email = "test@example.com";

            // Act - Generar 3 intentos fallidos
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);

            // Assert - Verificar que el contador se reseteó a 0 después del bloqueo
            var lockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            lockout.FailedAttempts.Should().Be(0);
        }

        #endregion

        #region IsAccountLockedAsync Tests

        [Fact]
        public async Task IsAccountLockedAsync_NoLockoutRecord_ReturnsNotLocked()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = await _securityDAL.IsAccountLockedAsync(email);

            // Assert
            result.IsLocked.Should().BeFalse();
            result.LockoutEnd.Should().BeNull();
        }

        [Fact]
        public async Task IsAccountLockedAsync_ActiveLockout_ReturnsLocked()
        {
            // Arrange
            var email = "test@example.com";
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);

            // Act
            var result = await _securityDAL.IsAccountLockedAsync(email);

            // Assert
            result.IsLocked.Should().BeTrue();
            result.LockoutEnd.Should().NotBeNull();
        }

        [Fact]
        public async Task IsAccountLockedAsync_ExpiredLockout_ClearsLockoutAndReturnsNotLocked()
        {
            // Arrange
            var email = "test@example.com";
            var lockout = new AccountLockoutCLS
            {
                Email = email,
                FailedAttempts = 0,
                LockoutEnd = DateTime.UtcNow.AddMinutes(-1), // Expirado
                LastAttempt = DateTime.UtcNow.AddMinutes(-31)
            };
            _context.ACCOUNT_LOCKOUTS.Add(lockout);
            await _context.SaveChangesAsync();

            // Act
            var result = await _securityDAL.IsAccountLockedAsync(email);

            // Assert
            result.IsLocked.Should().BeFalse();
            result.LockoutEnd.Should().BeNull();
            
            // Verificar que se limpió en la base de datos
            var updatedLockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            updatedLockout.LockoutEnd.Should().BeNull();
            updatedLockout.FailedAttempts.Should().Be(0);
        }

        #endregion

        #region ResetFailedAttemptsAsync Tests

        [Fact]
        public async Task ResetFailedAttemptsAsync_WithExistingAttempts_ClearsCounter()
        {
            // Arrange
            var email = "test@example.com";
            await _securityDAL.RecordFailedAttemptAsync(email);
            await _securityDAL.RecordFailedAttemptAsync(email);

            // Act
            await _securityDAL.ResetFailedAttemptsAsync(email);

            // Assert
            var lockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            lockout.FailedAttempts.Should().Be(0);
            lockout.LockoutEnd.Should().BeNull();
        }

        [Fact]
        public async Task ResetFailedAttemptsAsync_NoExistingRecord_DoesNotThrow()
        {
            // Arrange
            var email = "nonexistent@example.com";

            // Act
            Func<Task> act = async () => await _securityDAL.ResetFailedAttemptsAsync(email);

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion

        #region GetRecentFailedAttemptsCountAsync Tests

        [Fact]
        public async Task GetRecentFailedAttemptsCountAsync_WithinTimeWindow_CountsAttempts()
        {
            // Arrange
            var email = "test@example.com";
            await _securityDAL.LogLoginAttemptAsync(email, false);
            await _securityDAL.LogLoginAttemptAsync(email, false);
            await _securityDAL.LogLoginAttemptAsync(email, false);

            // Act
            var count = await _securityDAL.GetRecentFailedAttemptsCountAsync(email, 30);

            // Assert
            count.Should().Be(3);
        }

        [Fact]
        public async Task GetRecentFailedAttemptsCountAsync_SuccessfulAttempts_NotCounted()
        {
            // Arrange
            var email = "test@example.com";
            await _securityDAL.LogLoginAttemptAsync(email, false);
            await _securityDAL.LogLoginAttemptAsync(email, true); // Exitoso
            await _securityDAL.LogLoginAttemptAsync(email, false);

            // Act
            var count = await _securityDAL.GetRecentFailedAttemptsCountAsync(email, 30);

            // Assert
            count.Should().Be(2); // Solo los fallidos
        }

        #endregion

        #region Integration Scenarios

        [Fact]
        public async Task CompleteLoginFlow_ThreeFailedThenSuccess_ResetsLockout()
        {
            // Arrange
            var email = "test@example.com";

            // Act - 3 intentos fallidos (bloqueo)
            await _securityDAL.LogLoginAttemptAsync(email, false);
            await _securityDAL.RecordFailedAttemptAsync(email);
            
            await _securityDAL.LogLoginAttemptAsync(email, false);
            await _securityDAL.RecordFailedAttemptAsync(email);
            
            await _securityDAL.LogLoginAttemptAsync(email, false);
            var lockoutResult = await _securityDAL.RecordFailedAttemptAsync(email);

            // Assert - Cuenta bloqueada
            lockoutResult.IsLocked.Should().BeTrue();

            // Simular que el bloqueo expiró (en producción sería después de 30 min)
            var lockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            lockout.LockoutEnd = DateTime.UtcNow.AddMinutes(-1);
            await _context.SaveChangesAsync();

            // Act - Login exitoso después de expiración
            var isLocked = await _securityDAL.IsAccountLockedAsync(email);
            isLocked.IsLocked.Should().BeFalse();
            
            await _securityDAL.LogLoginAttemptAsync(email, true);
            await _securityDAL.ResetFailedAttemptsAsync(email);

            // Assert - Contador reseteado
            var finalLockout = await _context.ACCOUNT_LOCKOUTS.FirstAsync(x => x.Email == email);
            finalLockout.FailedAttempts.Should().Be(0);
            finalLockout.LockoutEnd.Should().BeNull();
        }

        #endregion
    }
}
