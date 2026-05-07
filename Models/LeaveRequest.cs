using System.ComponentModel.DataAnnotations;
using SmartEMS.API.Models;

namespace SmartEMS.API.Models;

public class LeaveRequest : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public LeaveType LeaveType { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public Employee Employee { get; set; } = null!;
}