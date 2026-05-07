using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Department;
namespace SmartEMS.API.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DepartmentResponseDto>> GetAllAsync()
    {
        var departments = await _repository.GetAllAsync();

        return departments.Select(d => new DepartmentResponseDto
        {
            Id = d.Id,
            Code = d.Code,
            Name = d.Name
        }).ToList();
    }
}