using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.Models;

public class AttendanceCorrectionRequest : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime Date { get; set; }

    public string Reason { get; set; } = string.Empty;

    public CorrectionStatus Status { get; set; } = CorrectionStatus.Pending;

    public Employee Employee { get; set; } = null!;
}