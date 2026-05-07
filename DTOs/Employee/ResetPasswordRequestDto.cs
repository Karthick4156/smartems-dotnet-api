namespace SmartEMS.API.DTOs.Employee;

public class ResetPasswordRequestDto
{
    public string? Email { get; set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}