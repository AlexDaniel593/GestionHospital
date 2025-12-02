using CapaDatos;
using CapaEntidad;
using GestionHospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionHospital.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PacienteDAL _pacienteDAL;
        private readonly SecurityDAL _securityDAL;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            PacienteDAL pacienteDAL,
            SecurityDAL securityDAL,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _pacienteDAL = pacienteDAL;
            _securityDAL = securityDAL;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get client IP and User Agent for security logging
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                // Check if account is locked
                var (isLocked, lockoutEnd) = await _securityDAL.IsAccountLockedAsync(model.Email);
                if (isLocked && lockoutEnd.HasValue)
                {
                    var remainingMinutes = Math.Ceiling((lockoutEnd.Value - DateTime.UtcNow).TotalMinutes);
                    
                    // Log the blocked attempt
                    await _securityDAL.LogLoginAttemptAsync(
                        model.Email, 
                        false, 
                        ipAddress, 
                        userAgent, 
                        $"Cuenta bloqueada. Tiempo restante: {remainingMinutes} minutos");

                    _logger.LogWarning("Intento de inicio de sesión bloqueado para cuenta: {Email} desde IP: {IpAddress}", 
                        model.Email, ipAddress);

                    ModelState.AddModelError(string.Empty, 
                        $"La cuenta está bloqueada debido a múltiples intentos fallidos de inicio de sesión. Por favor, intente nuevamente en {remainingMinutes} minutos.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    // Reset failed attempts on successful login
                    await _securityDAL.ResetFailedAttemptsAsync(model.Email);
                    
                    // Log successful login
                    await _securityDAL.LogLoginAttemptAsync(model.Email, true, ipAddress, userAgent);
                    
                    _logger.LogInformation("Inicio de sesión exitoso para usuario: {Email} desde IP: {IpAddress}", 
                        model.Email, ipAddress);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Record failed attempt
                    var (accountLocked, newLockoutEnd) = await _securityDAL.RecordFailedAttemptAsync(model.Email);
                    
                    string failureReason = result.IsLockedOut ? "Cuenta bloqueada por el sistema" :
                                         result.IsNotAllowed ? "Inicio de sesión no permitido" :
                                         result.RequiresTwoFactor ? "Requiere autenticación de dos factores" :
                                         "Credenciales inválidas";

                    // Log failed attempt
                    await _securityDAL.LogLoginAttemptAsync(
                        model.Email, 
                        false, 
                        ipAddress, 
                        userAgent, 
                        failureReason);

                    _logger.LogWarning("Intento de inicio de sesión fallido para usuario: {Email} desde IP: {IpAddress}. Razón: {Reason}", 
                        model.Email, ipAddress, failureReason);

                    if (accountLocked && newLockoutEnd.HasValue)
                    {
                        _logger.LogWarning("Cuenta bloqueada debido a múltiples intentos fallidos: {Email}", model.Email);
                        ModelState.AddModelError(string.Empty, 
                            "Demasiados intentos fallidos de inicio de sesión. Su cuenta ha sido bloqueada por 30 minutos.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido.");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Ensure the "Patient" role exists
                    if (!await _roleManager.RoleExistsAsync("Patient"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Patient"));
                    }

                    // Assign the "Patient" role to the user
                    await _userManager.AddToRoleAsync(user, "Patient");

                    // Save patient data in the PACIENTES table
                    var paciente = new PacienteCLS
                    {
                        nombre = model.Nombre,
                        apellido = model.Apellido,
                        fechaNacimiento = model.FechaNacimiento,
                        telefono = model.Telefono,
                        email = model.Email,
                        direccion = model.Direccion,
                        BHABILITADO = 1
                    };
                    _pacienteDAL.GuardarPaciente(paciente);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
