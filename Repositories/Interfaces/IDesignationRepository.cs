using SmartEMS.API.Models;

public interface IDesignationRepository
{
    Task<List<Designation>> GetByDepartmentIdAsync(int departmentId);
}