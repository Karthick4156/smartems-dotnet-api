namespace SmartEMS.API.DTOs.Dashboard;
public class DashboardSummaryDto
{
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }
    public int PendingLeaves { get; set; }
    public int PendingCorrections { get; set; }
}