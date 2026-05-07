using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartEMS.API.Repositories.Implementations;

public class LeaveRepository : ILeaveRepository
{
    private readonly AppDbContext _context;

    public LeaveRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LeaveRequest leave)
    {
        await _context.LeaveRequests.AddAsync(leave);
    }

    public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.LeaveRequests
            .Where(l => l.EmployeeId == employeeId)
            .OrderByDescending(l => l.FromDate)
            .ToListAsync();
    }

    public async Task<List<LeaveRequest>> GetAllAsync()
    {
        return await _context.LeaveRequests
            .Include(l => l.Employee)
            .OrderByDescending(l => l.FromDate)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id)
    {
        return await _context.LeaveRequests
            .Include(l => l.Employee)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _context.LeaveRequests
            .CountAsync(l => l.Status == LeaveStatus.Pending);
    }
}