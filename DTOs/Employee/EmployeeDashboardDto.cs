namespace SmartEMS.API.DTOs.Employee;

public class EmployeeDashboardDto
{
    public string TodayStatus { get; set; } = string.Empty;
    public DateTime? PunchIn { get; set; }
    public DateTime? PunchOut { get; set; }
    public double WorkHours { get; set; }

    public int LeaveTaken { get; set; }
    public int PendingLeaves { get; set; }

    public int PendingCorrections { get; set; }
}