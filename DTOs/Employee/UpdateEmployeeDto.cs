namespace SmartEMS.API.DTOs.Employee;

public class UpdateEmployeeRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
    public DateTime JoiningDate { get; set; }
}