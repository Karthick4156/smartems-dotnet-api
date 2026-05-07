using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.DTOs.Employee;

public class CreateEmployeeRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
    ErrorMessage = "Password must contain letters and numbers")]
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }

    public DateTime JoiningDate { get; set; }
}