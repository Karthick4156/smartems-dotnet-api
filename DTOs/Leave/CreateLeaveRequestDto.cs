using SmartEMS.API.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.DTOs.Leave;

public class CreateLeaveRequestDto
{
    [Required]
    public LeaveType LeaveType { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty;
}