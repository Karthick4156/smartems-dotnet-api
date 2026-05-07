using SmartEMS.API.DTOs.Designation;

public interface IDesignationService
{
    Task<List<DesignationResponseDto>> GetByDepartmentIdAsync(int departmentId);
}