using System.ComponentModel.DataAnnotations;

namespace SmartEMS.API.Models;

public class Department : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty; // HR, IT, FINANCE

    [Required]
    public string Name { get; set; } = string.Empty; // Human Resource, IT, Finance
    
    public List<Designation> Designations { get; set; } = new();

}