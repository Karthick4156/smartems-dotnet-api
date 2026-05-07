using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.DTOs.Auth;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
ErrorMessage = "Password must contain letters and numbers")]
    public string Password { get; set; } = string.Empty;
}