using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartEMS.API.Models;

public class Attendance : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime? PunchInTime { get; set; }
    public DateTime? PunchOutTime { get; set; }

    public double WorkHours { get; set; } = 0;
    public string Status { get; set; } = "Pending"; // Pending, FullDay, HalfDay, Absent

    // FK
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}