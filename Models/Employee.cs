using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartEMS.API.Models;

public class Employee : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string EmployeeCode { get; set; } = string.Empty; // EMS001

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public Department Department { get; set; } = null!;

    [Required]
    public int DesignationId { get; set; }

    [Required]
    public Designation Designation { get; set; } = null!;

    [Required]
    public DateTime JoiningDate { get; set; }

    public string? Phone { get; set; }
    public string? Address { get; set; }

    // Leave balances
    public int PaidLeaveBalance { get; set; } = 12;
    public int SickLeaveBalance { get; set; } = 6;

    // FK
    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}