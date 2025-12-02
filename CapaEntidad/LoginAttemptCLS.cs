using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class LoginAttemptCLS
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime AttemptTime { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        [StringLength(45)]
        public string? IpAddress { get; set; }

        [StringLength(500)]
        public string? UserAgent { get; set; }

        [StringLength(500)]
        public string? FailureReason { get; set; }
    }
}
