using CapaDatos;
using CapaEntidad;
using FluentAssertions;
using GestionHospital.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GestionHospital.Tests.Integration
{
    /// <summary>
    /// Pruebas de integración que validan el flujo completo desde API hasta base de datos
    /// </summary>
    public class SecurityIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SecurityIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remover el DbContext real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Agregar DbContext con base de datos en memoria para pruebas
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task LoginFlow_ThreeFailedAttempts_CreatesLockoutRecord()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var securityDAL = scope.ServiceProvider.GetRequiredService<SecurityDAL>();

            var testEmail = $"integrationtest{Guid.NewGuid()}@example.com";

            // Act - Simular 3 intentos fallidos
            for (int i = 0; i < 3; i++)
            {
                await securityDAL.LogLoginAttemptAsync(testEmail, false, "192.168.1.1", "Test Browser");
                await securityDAL.RecordFailedAttemptAsync(testEmail);
            }

            // Assert - Verificar en base de datos
            var loginAttempts = await context.LOGIN_ATTEMPTS
                .Where(x => x.Email == testEmail)
                .ToListAsync();
            loginAttempts.Should().HaveCount(3);
            loginAttempts.Should().OnlyContain(x => !x.IsSuccessful);

            var lockout = await context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == testEmail);
            lockout.Should().NotBeNull();
            lockout.LockoutEnd.Should().NotBeNull();
            lockout.LockoutEnd.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(30), TimeSpan.FromMinutes(1));
        }

        [Fact]
        public async Task LoginFlow_SuccessfulLoginAfterFailures_ResetsCounter()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var securityDAL = scope.ServiceProvider.GetRequiredService<SecurityDAL>();

            var testEmail = $"integrationtest{Guid.NewGuid()}@example.com";

            // Act - 2 intentos fallidos
            await securityDAL.RecordFailedAttemptAsync(testEmail);
            await securityDAL.RecordFailedAttemptAsync(testEmail);

            // Verificar estado antes del login exitoso
            var lockoutBefore = await context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == testEmail);
            lockoutBefore.Should().NotBeNull();
            lockoutBefore.FailedAttempts.Should().Be(2);

            // Login exitoso
            await securityDAL.LogLoginAttemptAsync(testEmail, true);
            await securityDAL.ResetFailedAttemptsAsync(testEmail);

            // Assert - Verificar reset
            var lockoutAfter = await context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == testEmail);
            lockoutAfter.FailedAttempts.Should().Be(0);
            lockoutAfter.LockoutEnd.Should().BeNull();

            var successfulAttempt = await context.LOGIN_ATTEMPTS
                .Where(x => x.Email == testEmail && x.IsSuccessful)
                .FirstOrDefaultAsync();
            successfulAttempt.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginFlow_ExpiredLockout_AllowsLogin()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var securityDAL = scope.ServiceProvider.GetRequiredService<SecurityDAL>();

            var testEmail = $"integrationtest{Guid.NewGuid()}@example.com";

            // Crear bloqueo expirado
            var expiredLockout = new AccountLockoutCLS
            {
                Email = testEmail,
                FailedAttempts = 0,
                LockoutEnd = DateTime.UtcNow.AddMinutes(-5), // Expirado hace 5 minutos
                LastAttempt = DateTime.UtcNow.AddMinutes(-35)
            };
            context.ACCOUNT_LOCKOUTS.Add(expiredLockout);
            await context.SaveChangesAsync();

            // Act - Verificar estado de bloqueo
            var (isLocked, lockoutEnd) = await securityDAL.IsAccountLockedAsync(testEmail);

            // Assert
            isLocked.Should().BeFalse();
            lockoutEnd.Should().BeNull();

            // Verificar que se limpió en la base de datos
            var updatedLockout = await context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == testEmail);
            updatedLockout.LockoutEnd.Should().BeNull();
            updatedLockout.FailedAttempts.Should().Be(0);
        }

        [Fact]
        public async Task SecurityDAL_LogsIpAndUserAgent_PersistsToDatabase()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var securityDAL = scope.ServiceProvider.GetRequiredService<SecurityDAL>();

            var testEmail = $"integrationtest{Guid.NewGuid()}@example.com";
            var ipAddress = "203.0.113.42";
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Integration Test";

            // Act
            await securityDAL.LogLoginAttemptAsync(
                testEmail, 
                false, 
                ipAddress, 
                userAgent, 
                "Test failure reason");

            // Assert
            var loggedAttempt = await context.LOGIN_ATTEMPTS
                .Where(x => x.Email == testEmail)
                .FirstOrDefaultAsync();

            loggedAttempt.Should().NotBeNull();
            loggedAttempt.IpAddress.Should().Be(ipAddress);
            loggedAttempt.UserAgent.Should().Be(userAgent);
            loggedAttempt.FailureReason.Should().Be("Test failure reason");
            loggedAttempt.IsSuccessful.Should().BeFalse();
            loggedAttempt.AttemptTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task GetRecentFailedAttempts_FiltersCorrectly()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var securityDAL = scope.ServiceProvider.GetRequiredService<SecurityDAL>();

            var testEmail = $"integrationtest{Guid.NewGuid()}@example.com";

            // Crear intentos en diferentes momentos
            var oldAttempt = new LoginAttemptCLS
            {
                Email = testEmail,
                IsSuccessful = false,
                AttemptTime = DateTime.UtcNow.AddMinutes(-45) // Fuera de la ventana de 30 min
            };
            context.LOGIN_ATTEMPTS.Add(oldAttempt);

            // Intentos recientes
            await securityDAL.LogLoginAttemptAsync(testEmail, false);
            await securityDAL.LogLoginAttemptAsync(testEmail, false);
            await securityDAL.LogLoginAttemptAsync(testEmail, true); // Exitoso, no debe contar

            await context.SaveChangesAsync();

            // Act
            var recentFailedCount = await securityDAL.GetRecentFailedAttemptsCountAsync(testEmail, 30);

            // Assert
            recentFailedCount.Should().Be(2); // Solo los 2 fallidos recientes
        }
    }
}
