using BCrypt.Net;
using SmartEMS.API.DTOs.Employee;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Data;
using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Models;
public class EmployeeSelfService : IEmployeeSelfService
{
    private readonly IUserRepository _userRepo;
    private readonly AppDbContext _context;

    public EmployeeSelfService(IUserRepository userRepo, AppDbContext context)
    {
        _userRepo = userRepo;
        _context = context;
    }

    public async Task<EmployeeProfileResponseDto> GetProfileAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var e = user.Employee ?? throw new Exception("Employee not found");

        return new EmployeeProfileResponseDto
        {
            EmployeeCode = e.EmployeeCode,
            UserId = user.UserId,
            Name = e.Name,
            Email = user.Email,
            Department = e.Department.Code,
            Designation = e.Designation.Name,
            Phone = e.Phone,
            Address = e.Address,
            PaidLeaveBalance = e.PaidLeaveBalance,
            SickLeaveBalance = e.SickLeaveBalance
        };
    }

    public async Task UpdateProfileAsync(string email, UpdateProfileRequestDto request)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        employee.Phone = request.Phone ?? employee.Phone;
        employee.Address = request.Address ?? employee.Address;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(string email, ChangePasswordRequestDto request)
    {
        var user = await _userRepo.GetByEmailAsync(email)
            ?? throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new Exception("Invalid current password");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        await _context.SaveChangesAsync();
    }

    public async Task RequestCorrectionAsync(string email, DateTime date, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new Exception("Reason is required");

        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        // Check if attendance exists
        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(a =>
                a.EmployeeId == employee.Id &&
                a.Date.Date == date.Date);

        if (attendance == null)
            throw new Exception("Attendance record not found for selected date");

        // Prevent duplicate requests
        var existing = await _context.AttendanceCorrectionRequests
            .FirstOrDefaultAsync(r =>
                r.EmployeeId == employee.Id &&
                r.Date.Date == date.Date &&
                r.Status == CorrectionStatus.Pending);

        if (existing != null)
            throw new Exception("Correction request already exists");

        var request = new AttendanceCorrectionRequest
        {
            EmployeeId = employee.Id,
            Date = date.Date,
            Reason = reason,
            Status = CorrectionStatus.Pending
        };

        await _context.AttendanceCorrectionRequests.AddAsync(request);
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeDashboardDto> GetDashboardAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        var today = DateTime.UtcNow.Date;

        // 📊 TODAY ATTENDANCE
        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(a =>
                a.EmployeeId == employee.Id &&
                a.Date.Date == today);

        // 📊 LEAVES
        var leaves = await _context.LeaveRequests
            .Where(l => l.EmployeeId == employee.Id)
            .ToListAsync();

        // 📊 CORRECTIONS
        var corrections = await _context.AttendanceCorrectionRequests
            .Where(c => c.EmployeeId == employee.Id)
            .ToListAsync();

        if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
        {
            return new EmployeeDashboardDto
            {
                TodayStatus = "Weekend"
            };
        }

        return new EmployeeDashboardDto
        {
            TodayStatus = attendance == null ? "Not Marked"
                        : attendance.PunchOutTime == null ? "In Progress"
                        : attendance.Status,
            PunchIn = attendance?.PunchInTime,
            PunchOut = attendance?.PunchOutTime,
            WorkHours = attendance?.WorkHours ?? 0,

            LeaveTaken = leaves.Count(l => l.Status.ToString().ToLower() == "approved"),
            PendingLeaves = leaves.Count(l => l.Status.ToString().ToLower() == "pending"),
            PendingCorrections = corrections.Count(c => c.Status == CorrectionStatus.Pending)
        };
    }
}