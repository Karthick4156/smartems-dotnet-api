using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.DTOs.Employee;

public class EmployeeListResponseDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? InactiveReason { get; set; }
    [Required]
    public int DepartmentId { get; set; }     // ✅ ADD
    public string Department { get; set; } = string.Empty;

    public int DesignationId { get; set; }    // ✅ ADD
    public string Designation { get; set; } = string.Empty;

    public DateTime JoiningDate { get; set; } // ✅ ADD

    public string Status { get; set; } = string.Empty;
}