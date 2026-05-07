namespace SmartEMS.API.DTOs.Employee;
public class ToggleStatusDto
{
    public bool IsActive { get; set; }

    public string? Reason { get; set; }
}