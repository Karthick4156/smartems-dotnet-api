using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Models;

public class CorrectionRepository : ICorrectionRepository
{
    private readonly AppDbContext _context;

    public CorrectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _context.AttendanceCorrectionRequests
            .CountAsync(c => c.Status == CorrectionStatus.Pending);
    }

    public async Task<List<object>> GetAllAsync()
    {
        return await _context.AttendanceCorrectionRequests
            .Include(c => c.Employee)
            .OrderByDescending(c => c.Date)
            .Select(c => new
            {
                c.Id,
                c.Date,
                c.Reason,
                Status = c.Status.ToString(),
                EmployeeName = c.Employee.Name,
                EmployeeCode = c.Employee.EmployeeCode
            })
            .ToListAsync<object>();
    }
}