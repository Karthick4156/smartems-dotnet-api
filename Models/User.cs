using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.Models;

public class User : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty; // ADM001 / EMP001

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [Required]
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public string? InactiveReason { get; set; }

    public Employee? Employee { get; set; }
}