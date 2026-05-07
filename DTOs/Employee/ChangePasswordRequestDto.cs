using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.DTOs.Employee;

public class ChangePasswordRequestDto
{
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
        ErrorMessage = "Password must contain letters and numbers")]
    public string NewPassword { get; set; } = string.Empty;
}