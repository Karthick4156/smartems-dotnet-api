using SmartEMS.API.Models;

namespace SmartEMS.API.Repositories.Interfaces;

public interface ILeaveRepository
{
    Task AddAsync(LeaveRequest leave);
    Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
    Task<List<LeaveRequest>> GetAllAsync();
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<int> GetPendingCountAsync();
}