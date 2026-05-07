using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartEMS.API.Models;

public class Designation : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    // FK → Department
    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
}