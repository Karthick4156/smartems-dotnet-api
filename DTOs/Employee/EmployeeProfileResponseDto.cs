namespace SmartEMS.API.DTOs.Employee;

public class EmployeeProfileResponseDto
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }

    public int PaidLeaveBalance { get; set; }
    public int SickLeaveBalance { get; set; }
}