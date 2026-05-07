using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.Data;
using SmartEMS.API.Models;
using SmartEMS.API.DTOs.Attendance;
using SmartEMS.API.DTOs.Correction;
using Microsoft.EntityFrameworkCore;

namespace SmartEMS.API.Services.Implementations;

public class AttendanceService : IAttendanceService
{
    private readonly IUserRepository _userRepo;
    private readonly IAttendanceRepository _attendanceRepo;
    private readonly AppDbContext _context;

    public AttendanceService(
        IUserRepository userRepo,
        IAttendanceRepository attendanceRepo,
        AppDbContext context)
    {
        _userRepo = userRepo;
        _attendanceRepo = attendanceRepo;
        _context = context;
    }

    public async Task PunchInAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        var today = DateTime.UtcNow.Date;

        var existing = await _attendanceRepo.GetTodayAsync(employee.Id, today);

        if (existing != null)
        {
            if (existing.PunchInTime != null)
                throw new Exception("Already punched in");

            existing.PunchInTime = DateTime.UtcNow;
            existing.Status = "InProgress";
        }
        else
        {
            var attendance = new Attendance
            {
                EmployeeId = employee.Id,
                Date = today,
                PunchInTime = DateTime.UtcNow,
                Status = "InProgress" // ✅ IMPORTANT
            };

            await _attendanceRepo.AddAsync(attendance);
        }

        await _context.SaveChangesAsync();
    }

    public async Task PunchOutAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        var today = DateTime.UtcNow.Date;

        var attendance = await _attendanceRepo.GetTodayAsync(employee.Id, today)
            ?? throw new Exception("Punch in first");

        if (attendance.PunchOutTime != null)
            throw new Exception("Already punched out");

        if (attendance.PunchInTime == null)
            throw new Exception("Invalid punch-in data");

        var punchOutTime = DateTime.UtcNow;

        var workDuration = punchOutTime - attendance.PunchInTime.Value;

        if (workDuration.TotalMinutes < 0)
            throw new Exception("Invalid time calculation");

        attendance.PunchOutTime = punchOutTime;
        attendance.WorkHours = Math.Round(workDuration.TotalHours, 2);

        if (attendance.WorkHours >= 7)
            attendance.Status = "FullDay";
        else if (attendance.WorkHours >= 4)
            attendance.Status = "HalfDay";
        else
            attendance.Status = "Absent";

        await _context.SaveChangesAsync();
    }
    public async Task<List<AttendanceResponseDto>> GetMyAttendanceAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee
            ?? throw new Exception("Employee not found");

        var records = await _context.Attendances
            .Where(a => a.EmployeeId == employee.Id)
            .OrderByDescending(a => a.Date)
            .Select(a => new AttendanceResponseDto
            {
                Date = a.Date.Date, // ✅ remove time part
                PunchIn = a.PunchInTime,
                PunchOut = a.PunchOutTime,
                WorkHours = a.WorkHours,
                Status = a.Status.ToString() // ✅ string for UI
            })
            .ToListAsync();

        return records;
    }

    public async Task ApproveCorrectionAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var request = await _context.AttendanceCorrectionRequests.FindAsync(id)
                ?? throw new Exception("Request not found");

            if (request.Status != CorrectionStatus.Pending)
                throw new Exception("Already processed");

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == request.EmployeeId &&
                    a.Date.Date == request.Date.Date); // ✅ FIX

            if (attendance == null)
                throw new Exception("Attendance not found");

            attendance.WorkHours = 8;
            attendance.Status = "FullDay";

            request.Status = CorrectionStatus.Approved;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RejectCorrectionAsync(int id)
    {
        var request = await _context.AttendanceCorrectionRequests.FindAsync(id)
            ?? throw new Exception("Request not found");

        if (request.Status != CorrectionStatus.Pending)
            throw new Exception("Already processed");

        request.Status = CorrectionStatus.Rejected;

        await _context.SaveChangesAsync();
    }
    public async Task<List<CorrectionResponseDto>> GetAllCorrectionsAsync()
    {
        return await _context.AttendanceCorrectionRequests
            .Include(c => c.Employee)
            .OrderByDescending(c => c.Date)
            .Select(c => new CorrectionResponseDto
            {
                Id = c.Id,
                Date = c.Date,
                Reason = c.Reason,
                Status = c.Status.ToString(),
                EmployeeName = c.Employee.Name,
                EmployeeCode = c.Employee.EmployeeCode
            })
            .ToListAsync();
    }
}