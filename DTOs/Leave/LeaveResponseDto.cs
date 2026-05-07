namespace SmartEMS.API.DTOs.Leave;

public class LeaveResponseDto
{
    public int Id { get; set; }
    public string? EmployeeCode {get; set;}
    public string? Name {get; set;}
    public string LeaveType { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Status { get; set; } = string.Empty;
}