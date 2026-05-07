using SmartEMS.API.DTOs.Department;
using SmartEMS.API.Models;

namespace SmartEMS.API.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentResponseDto>> GetAllAsync();
}