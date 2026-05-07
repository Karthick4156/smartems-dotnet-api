using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Data;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Leave;
using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Exceptions;

namespace SmartEMS.API.Services.Implementations;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepo;
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepo;

    public LeaveService(ILeaveRepository repository, AppDbContext context, IUserRepository userRepo)
    {
        _leaveRepo = repository;
        _context = context;
        _userRepo = userRepo;
    }

    public async Task ApplyLeaveAsync(string email, CreateLeaveRequestDto request)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee
            ?? throw new Exception("Employee not found");

        var fromDate = request.FromDate.Date;
        var toDate = request.ToDate.Date;

        // ✅ VALIDATION: date range
        if (fromDate > toDate)
            throw new BadRequestException("Invalid date range");

        // ✅ OVERLAP CHECK (CLEAN + OPTIMIZED)
        var exists = await _context.LeaveRequests
            .AnyAsync(l =>
                l.EmployeeId == employee.Id &&
                l.Status != LeaveStatus.Rejected &&
                fromDate <= l.ToDate &&
                toDate >= l.FromDate
            );

        if (exists)
            throw new BadRequestException("You already have a leave request for this date range");

        // ✅ CREATE
        var leave = new LeaveRequest
        {
            EmployeeId = employee.Id,
            LeaveType = request.LeaveType,
            FromDate = fromDate,
            ToDate = toDate,
            Reason = request.Reason,
            Status = LeaveStatus.Pending
        };

        await _leaveRepo.AddAsync(leave);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LeaveResponseDto>> GetMyLeavesAsync(string email)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(email)
            ?? throw new Exception("User not found");

        var employee = user.Employee ?? throw new Exception("Employee not found");

        var leaves = await _leaveRepo.GetByEmployeeIdAsync(employee.Id);

        return leaves.Select(l => new LeaveResponseDto
        {
            Id = l.Id,
            LeaveType = l.LeaveType.ToString(),   // ✅ FIX
            FromDate = l.FromDate,
            ToDate = l.ToDate,
            Status = l.Status.ToString()          // ✅ FIX
        }).ToList();
    }

    public async Task<List<LeaveResponseDto>> GetAllAsync()
    {
        var leaves = await _leaveRepo.GetAllAsync();

        return leaves.Select(l => new LeaveResponseDto
        {
            Id = l.Id,
            EmployeeCode = l.Employee.EmployeeCode,
            Name = l.Employee.Name,
            LeaveType = l.LeaveType.ToString(),  // ✅ FIX
            FromDate = l.FromDate,
            ToDate = l.ToDate,
            Status = l.Status.ToString()         // ✅ FIX
        }).ToList();
    }

    public async Task ApproveAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var leave = await _leaveRepo.GetByIdAsync(id)
                ?? throw new Exception("Leave not found");

            if (leave.Status != LeaveStatus.Pending)
                throw new Exception("Already processed");

            var days = (leave.ToDate - leave.FromDate).Days + 1;

            var employee = await _context.Employees.FindAsync(leave.EmployeeId)
                ?? throw new Exception("Employee not found");

            if (leave.LeaveType == LeaveType.Paid)
            {
                if (employee.PaidLeaveBalance < days)
                    throw new Exception("Insufficient paid leave");

                employee.PaidLeaveBalance -= days;
            }
            else if (leave.LeaveType == LeaveType.Sick)
            {
                if (employee.SickLeaveBalance < days)
                    throw new Exception("Insufficient sick leave");

                employee.SickLeaveBalance -= days;
            }

            leave.Status = LeaveStatus.Approved;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync(); // ✅ IMPORTANT
            throw;
        }
    }

    public async Task RejectAsync(int id)
    {
        var leave = await _leaveRepo.GetByIdAsync(id)
            ?? throw new Exception("Leave not found");

        if (leave.Status != LeaveStatus.Pending)
            throw new Exception("Already processed");

        leave.Status = LeaveStatus.Rejected;

        await _context.SaveChangesAsync();
    }
}