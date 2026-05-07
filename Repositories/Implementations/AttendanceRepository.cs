using SmartEMS.API.Data;
using SmartEMS.API.Models;
using Microsoft.EntityFrameworkCore;


namespace SmartEMS.API.Repositories.Implementations;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _context;

    public AttendanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Attendance?> GetTodayAsync(int employeeId, DateTime date)
    {
        return await _context.Attendances
            .FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.Date.Date == date);
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
    }
}