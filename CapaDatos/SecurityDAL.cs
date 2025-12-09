using CapaEntidad;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SecurityDAL
    {
        private readonly ApplicationDbContext _context;

        public SecurityDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Log a login attempt (successful or failed)
        /// </summary>
        public async Task LogLoginAttemptAsync(string email, bool isSuccessful, string? ipAddress = null, 
            string? userAgent = null, string? failureReason = null)
        {
            var loginAttempt = new LoginAttemptCLS
            {
                Email = email,
                AttemptTime = DateTime.UtcNow,
                IsSuccessful = isSuccessful,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                FailureReason = failureReason
            };

            _context.LOGIN_ATTEMPTS.Add(loginAttempt);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get or create account lockout record for a user
        /// </summary>
        public async Task<AccountLockoutCLS> GetOrCreateLockoutRecordAsync(string email)
        {
            var lockout = await _context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == email);

            if (lockout == null)
            {
                lockout = new AccountLockoutCLS
                {
                    Email = email,
                    FailedAttempts = 0,
                    LastAttempt = DateTime.UtcNow
                };
                _context.ACCOUNT_LOCKOUTS.Add(lockout);
                await _context.SaveChangesAsync();
            }

            return lockout;
        }

        /// <summary>
        /// Record a failed login attempt and check if account should be locked
        /// </summary>
        public async Task<(bool IsLocked, DateTime? LockoutEnd)> RecordFailedAttemptAsync(string email)
        {
            var lockout = await GetOrCreateLockoutRecordAsync(email);

            lockout.FailedAttempts++;
            lockout.LastAttempt = DateTime.UtcNow;

            // Lock account after 3 failed attempts for 30 minutes
            if (lockout.FailedAttempts >= 3)
            {
                lockout.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                lockout.FailedAttempts = 0; // Reset counter after lockout
                
                // Send notification to administrator
                await SendLockoutNotificationAsync(email, lockout.LockoutEnd.Value);
            }

            await _context.SaveChangesAsync();

            return (lockout.IsLockedOut, lockout.LockoutEnd);
        }

        /// <summary>
        /// Send email notification to administrator when account is locked
        /// </summary>
        private async Task SendLockoutNotificationAsync(string userEmail, DateTime lockoutEnd)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    Console.WriteLine("SENDGRID_API_KEY no configurada, no se puede enviar notificación de bloqueo");
                    return;
                }

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("dalexis203@gmail.com", "Sistema de Gestión Hospitalaria");
                var to = new EmailAddress("dalexis203@gmail.com", "Administrador");
                var subject = "⚠️ Alerta de Seguridad: Cuenta Bloqueada por Intentos Fallidos";
                
                var htmlContent = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #dc3545; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                            .content {{ background-color: #f8f9fa; padding: 20px; border: 1px solid #dee2e6; }}
                            .alert-box {{ background-color: #fff3cd; border: 1px solid #ffc107; padding: 15px; margin: 15px 0; border-radius: 5px; }}
                            .info-table {{ width: 100%; border-collapse: collapse; margin: 15px 0; }}
                            .info-table td {{ padding: 8px; border-bottom: 1px solid #dee2e6; }}
                            .info-table td:first-child {{ font-weight: bold; width: 40%; }}
                            .footer {{ background-color: #e9ecef; padding: 15px; text-align: center; border-radius: 0 0 5px 5px; font-size: 12px; color: #6c757d; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>🔒 Alerta de Seguridad</h2>
                            </div>
                            <div class='content'>
                                <div class='alert-box'>
                                    <strong>⚠️ Atención:</strong> Se ha bloqueado una cuenta debido a múltiples intentos fallidos de inicio de sesión.
                                </div>
                                <h3>Detalles del Bloqueo:</h3>
                                <table class='info-table'>
                                    <tr>
                                        <td>Usuario:</td>
                                        <td><strong>{userEmail}</strong></td>
                                    </tr>
                                    <tr>
                                        <td>Fecha de bloqueo:</td>
                                        <td>{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC</td>
                                    </tr>
                                    <tr>
                                        <td>Bloqueo hasta:</td>
                                        <td>{lockoutEnd:dd/MM/yyyy HH:mm:ss} UTC</td>
                                    </tr>
                                    <tr>
                                        <td>Razón:</td>
                                        <td>3 intentos fallidos de inicio de sesión</td>
                                    </tr>
                                    <tr>
                                        <td>Duración:</td>
                                        <td>30 minutos</td>
                                    </tr>
                                </table>
                                <p><strong>Acción recomendada:</strong> Revise la tabla LOGIN_ATTEMPTS en la base de datos para más detalles sobre los intentos fallidos.</p>
                                <p>Si estos intentos no son legítimos, considere tomar medidas adicionales de seguridad.</p>
                            </div>
                            <div class='footer'>
                                <p>Este es un mensaje automático del Sistema de Gestión Hospitalaria</p>
                                <p>Por favor no responda a este correo</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                var response = await client.SendEmailAsync(msg);
                
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted && 
                    response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Error al enviar notificación de bloqueo: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar notificación de bloqueo: {ex.Message}");
            }
        }

        /// <summary>
        /// Reset failed attempts on successful login
        /// </summary>
        public async Task ResetFailedAttemptsAsync(string email)
        {
            var lockout = await _context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == email);

            if (lockout != null)
            {
                lockout.FailedAttempts = 0;
                lockout.LockoutEnd = null;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Check if account is currently locked
        /// </summary>
        public async Task<(bool IsLocked, DateTime? LockoutEnd)> IsAccountLockedAsync(string email)
        {
            var lockout = await _context.ACCOUNT_LOCKOUTS
                .FirstOrDefaultAsync(x => x.Email == email);

            if (lockout == null)
            {
                return (false, null);
            }

            // Check if lockout has expired
            if (lockout.LockoutEnd.HasValue && lockout.LockoutEnd.Value <= DateTime.UtcNow)
            {
                lockout.LockoutEnd = null;
                lockout.FailedAttempts = 0;
                await _context.SaveChangesAsync();
                return (false, null);
            }

            return (lockout.IsLockedOut, lockout.LockoutEnd);
        }

        /// <summary>
        /// Get recent failed login attempts for an email
        /// </summary>
        public async Task<int> GetRecentFailedAttemptsCountAsync(string email, int minutes = 30)
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-minutes);
            return await _context.LOGIN_ATTEMPTS
                .Where(x => x.Email == email && !x.IsSuccessful && x.AttemptTime >= cutoffTime)
                .CountAsync();
        }
    }
}
