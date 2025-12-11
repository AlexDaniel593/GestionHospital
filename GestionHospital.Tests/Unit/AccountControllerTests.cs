using CapaDatos;
using CapaEntidad;
using GestionHospital.Controllers;
using GestionHospital.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestionHospital.Tests.Unit
{
    /// <summary>
    /// Pruebas unitarias para AccountController
    /// Cobertura: Login, lockout handling, security logging integration
    /// </summary>
    public class AccountControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly SecurityDAL _securityDAL;
        private readonly PacienteDAL _pacienteDAL;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            // Crear DbContext con InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"AccountControllerTestDb_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);

            // Setup UserManager mock
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Setup SignInManager mock
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null, null, null, null);

            // Setup RoleManager mock
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null, null, null, null);

            // Crear DALs reales con el DbContext en memoria
            _securityDAL = new SecurityDAL(_context);
            _pacienteDAL = new PacienteDAL(_context);

            // Setup Logger mock
            _loggerMock = new Mock<ILogger<AccountController>>();

            // Create controller with DALs reales
            _controller = new AccountController(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _pacienteDAL,
                _securityDAL,
                _loggerMock.Object
            );

            // Setup HttpContext para simular IP y User-Agent
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");
            httpContext.Request.Headers["User-Agent"] = "Mozilla/5.0 Test Browser";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        #region Login GET Tests

        [Fact]
        public void Login_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Login();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Login POST Tests

        [Fact]
        public async Task Login_Post_ValidCredentials_RedirectsToHome()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "validuser@example.com",
                Password = "ValidPass@123",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
            redirectResult.ControllerName.Should().Be("Home");

            // Verificar que se guardó el intento exitoso en la base de datos
            var loginAttempt = await _context.LOGIN_ATTEMPTS
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.IsSuccessful);
            loginAttempt.Should().NotBeNull();
            loginAttempt!.IpAddress.Should().Be("192.168.1.1");

            // Verificar que se resetearon los intentos fallidos
            var lockout = await _context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            if (lockout != null)
            {
                lockout.FailedAttempts.Should().Be(0);
                lockout.LockoutEnd.Should().BeNull();
            }
        }

        [Fact]
        public async Task Login_Post_InvalidCredentials_RecordsFailedAttempt()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "WrongPassword",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.ViewData.ModelState.Should().ContainKey(string.Empty);

            // Verificar que se registró el intento fallido
            var loginAttempt = await _context.LOGIN_ATTEMPTS
                .FirstOrDefaultAsync(x => x.Email == model.Email && !x.IsSuccessful);
            loginAttempt.Should().NotBeNull();
            loginAttempt!.FailureReason.Should().Contain("Credenciales inválidas");

            // Verificar que se incrementó el contador de intentos fallidos
            var lockout = await _context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            lockout.Should().NotBeNull();
            lockout!.FailedAttempts.Should().Be(1);
        }

        [Fact]
        public async Task Login_Post_AccountLocked_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "locked@example.com",
                Password = "Test@123",
                RememberMe = false
            };

            // Crear bloqueo activo en la base de datos
            var lockout = new AccountLockoutCLS
            {
                Email = model.Email,
                FailedAttempts = 3,
                LockoutEnd = DateTime.UtcNow.AddMinutes(25),
                LastAttempt = DateTime.UtcNow
            };
            _context.ACCOUNT_LOCKOUTS.Add(lockout);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.ViewData.ModelState.Should().ContainKey(string.Empty);
            
            var errorMessage = viewResult.ViewData.ModelState[string.Empty]?.Errors.FirstOrDefault()?.ErrorMessage;
            errorMessage.Should().Contain("bloqueada");
        }

        [Fact]
        public async Task Login_Post_ThirdFailedAttempt_TriggersLockout()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "locktest@example.com",
                Password = "WrongPassword",
                RememberMe = false
            };

            // Simular 2 intentos fallidos previos
            var existingLockout = new AccountLockoutCLS
            {
                Email = model.Email,
                FailedAttempts = 2,
                LockoutEnd = null,
                LastAttempt = DateTime.UtcNow.AddMinutes(-5)
            };
            _context.ACCOUNT_LOCKOUTS.Add(existingLockout);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear(); // Limpiar el tracker para obtener datos frescos

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();

            // Verificar que se creó el bloqueo - recargar desde la base de datos
            _context.ChangeTracker.Clear();
            var updatedLockout = await _context.ACCOUNT_LOCKOUTS
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            updatedLockout.Should().NotBeNull();
            updatedLockout!.FailedAttempts.Should().Be(3);
            updatedLockout.LockoutEnd.Should().NotBeNull();
            updatedLockout.LockoutEnd.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(30), TimeSpan.FromMinutes(1));

            // Verificar que se registró el intento fallido
            var loginAttempts = await _context.LOGIN_ATTEMPTS
                .AsNoTracking()
                .Where(x => x.Email == model.Email && !x.IsSuccessful)
                .ToListAsync();
            loginAttempts.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region Security Logging Tests

        [Fact]
        public async Task Login_Post_LogsIpAddressAndUserAgent()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "iptest@example.com",
                Password = "Test@123",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var loginAttempt = await _context.LOGIN_ATTEMPTS
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            
            loginAttempt.Should().NotBeNull();
            loginAttempt!.IpAddress.Should().Be("192.168.1.1");
            loginAttempt.UserAgent.Should().Be("Mozilla/5.0 Test Browser");
        }

        #endregion
    }
}
