using CapaEntidad;
using Microsoft.EntityFrameworkCore;
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
            }

            await _context.SaveChangesAsync();

            return (lockout.IsLockedOut, lockout.LockoutEnd);
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
