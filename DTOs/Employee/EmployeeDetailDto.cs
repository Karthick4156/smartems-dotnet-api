namespace SmartEMS.API.DTOs.Employee;

public class EmployeeDetailResponseDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; } = string.Empty;

    public int DesignationId { get; set; }
    public string Designation { get; set; } = string.Empty;

    public DateTime JoiningDate { get; set; }

    public string Status { get; set; } = string.Empty;
}