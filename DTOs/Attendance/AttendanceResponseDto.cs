namespace SmartEMS.API.DTOs.Attendance;

public class AttendanceResponseDto
{
    public DateTime Date { get; set; }
    public DateTime? PunchIn { get; set; }
    public DateTime? PunchOut { get; set; }
    public double WorkHours { get; set; }
    public string Status { get; set; } = string.Empty;
}