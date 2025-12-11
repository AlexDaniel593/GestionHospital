using CapaDatos;
using GestionHospital.Controllers;
using GestionHospital.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestionHospital.Tests.Unit
{
    /// <summary>
    /// Pruebas unitarias para AccountController
    /// Cobertura: Login, lockout handling, security logging integration
    /// </summary>
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<SecurityDAL> _securityDALMock;
        private readonly Mock<PacienteDAL> _pacienteDALMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
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

            // Setup SecurityDAL mock
            var dbContextMock = new Mock<ApplicationDbContext>();
            _securityDALMock = new Mock<SecurityDAL>(dbContextMock.Object);

            // Setup PacienteDAL mock
            _pacienteDALMock = new Mock<PacienteDAL>(dbContextMock.Object);

            // Setup Logger mock
            _loggerMock = new Mock<ILogger<AccountController>>();

            // Create controller with mocks
            _controller = new AccountController(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _pacienteDALMock.Object,
                _securityDALMock.Object,
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
                Email = "test@example.com",
                Password = "Test1234!",
                RememberMe = false
            };

            _securityDALMock
                .Setup(x => x.IsAccountLockedAsync(model.Email))
                .ReturnsAsync((false, (DateTime?)null));

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _securityDALMock
                .Setup(x => x.ResetFailedAttemptsAsync(model.Email))
                .Returns(Task.CompletedTask);

            _securityDALMock
                .Setup(x => x.LogLoginAttemptAsync(model.Email, true, It.IsAny<string>(), It.IsAny<string>(), null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
            redirectResult.ControllerName.Should().Be("Home");

            // Verificar que se llamaron los métodos de seguridad
            _securityDALMock.Verify(x => x.ResetFailedAttemptsAsync(model.Email), Times.Once);
            _securityDALMock.Verify(x => x.LogLoginAttemptAsync(
                model.Email, true, It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
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

            _securityDALMock
                .Setup(x => x.IsAccountLockedAsync(model.Email))
                .ReturnsAsync((false, (DateTime?)null));

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            _securityDALMock
                .Setup(x => x.RecordFailedAttemptAsync(model.Email))
                .ReturnsAsync((false, (DateTime?)null));

            _securityDALMock
                .Setup(x => x.LogLoginAttemptAsync(
                    model.Email, false, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().Be(model);
            _controller.ModelState.Should().ContainKey(string.Empty);

            // Verificar que se registró el intento fallido
            _securityDALMock.Verify(x => x.RecordFailedAttemptAsync(model.Email), Times.Once);
            _securityDALMock.Verify(x => x.LogLoginAttemptAsync(
                model.Email, false, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Login_Post_AccountLocked_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "locked@example.com",
                Password = "Test1234!",
                RememberMe = false
            };

            var lockoutEnd = DateTime.UtcNow.AddMinutes(25);
            _securityDALMock
                .Setup(x => x.IsAccountLockedAsync(model.Email))
                .ReturnsAsync((true, lockoutEnd));

            _securityDALMock
                .Setup(x => x.LogLoginAttemptAsync(
                    model.Email, false, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().Be(model);
            _controller.ModelState.Should().ContainKey(string.Empty);
            
            var errorMessages = _controller.ModelState[string.Empty].Errors.Select(e => e.ErrorMessage);
            errorMessages.Should().Contain(msg => msg.Contains("bloqueada") && msg.Contains("25"));

            // Verificar que NO se intentó hacer sign in
            _signInManagerMock.Verify(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_Post_ThirdFailedAttempt_TriggersLockout()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "WrongPassword",
                RememberMe = false
            };

            var lockoutEnd = DateTime.UtcNow.AddMinutes(30);

            _securityDALMock
                .Setup(x => x.IsAccountLockedAsync(model.Email))
                .ReturnsAsync((false, (DateTime?)null));

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Simular que es el tercer intento fallido
            _securityDALMock
                .Setup(x => x.RecordFailedAttemptAsync(model.Email))
                .ReturnsAsync((true, lockoutEnd)); // Ahora está bloqueado

            _securityDALMock
                .Setup(x => x.LogLoginAttemptAsync(
                    model.Email, false, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            
            var errorMessages = _controller.ModelState[string.Empty].Errors.Select(e => e.ErrorMessage);
            errorMessages.Should().Contain(msg => msg.Contains("bloqueada") || msg.Contains("intentos fallidos"));

            _securityDALMock.Verify(x => x.RecordFailedAttemptAsync(model.Email), Times.Once);
        }

        #endregion

        #region Security Logging Tests

        [Fact]
        public async Task Login_Post_LogsIpAddressAndUserAgent()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "Test1234!",
                RememberMe = false
            };

            _securityDALMock
                .Setup(x => x.IsAccountLockedAsync(model.Email))
                .ReturnsAsync((false, (DateTime?)null));

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            string capturedIp = null;
            string capturedUserAgent = null;

            _securityDALMock
                .Setup(x => x.LogLoginAttemptAsync(model.Email, true, It.IsAny<string>(), It.IsAny<string>(), null))
                .Callback<string, bool, string, string, string>((email, success, ip, ua, reason) =>
                {
                    capturedIp = ip;
                    capturedUserAgent = ua;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _controller.Login(model);

            // Assert
            capturedIp.Should().Be("192.168.1.1");
            capturedUserAgent.Should().Be("Mozilla/5.0 Test Browser");
        }

        #endregion
    }
}
