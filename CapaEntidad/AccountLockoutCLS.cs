using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class AccountLockoutCLS
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int FailedAttempts { get; set; }

        public DateTime? LockoutEnd { get; set; }

        [Required]
        public DateTime LastAttempt { get; set; }

        public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    }
}
