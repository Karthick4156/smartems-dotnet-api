using SmartEMS.API.DTOs.Designation;

public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _repository;

    public DesignationService(IDesignationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DesignationResponseDto>> GetByDepartmentIdAsync(int departmentId)
    {
        var designation = await _repository.GetByDepartmentIdAsync(departmentId);
        return designation.Select(d => new DesignationResponseDto
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
    }
}