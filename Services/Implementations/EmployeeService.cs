using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using SmartEMS.API.DTOs.Employee;
using SmartEMS.API.Helpers;
using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.DTOs.Common;
using SmartEMS.API.DTOs.Dashboard;
using SmartEMS.API.Exceptions;
public class EmployeeService : IEmployeeService
{
    private readonly IUserRepository _userRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IDesignationRepository _designationRepo;
    private ILeaveRepository _leaveRepo;
    private ICorrectionRepository _correctionRepo;

    public EmployeeService(
        IUserRepository userRepo,
        IEmployeeRepository employeeRepo,
        IDepartmentRepository departmentRepo,
        IDesignationRepository designationRepo,
        ILeaveRepository leaveRepo,
        ICorrectionRepository correctionRepo)
    {
        _userRepo = userRepo;
        _employeeRepo = employeeRepo;
        _departmentRepo = departmentRepo;
        _designationRepo = designationRepo;
        _leaveRepo = leaveRepo;
        _correctionRepo = correctionRepo;
    }

    public async Task CreateEmployeeAsync(CreateEmployeeRequestDto request)
    {
        // 1. Validate Department
        var department = await _departmentRepo.GetByIdAsync(request.DepartmentId);
        if (department == null)
            throw new Exception("Invalid Department");

        // 2. Validate Designation
        var designation = await _designationRepo.GetByDepartmentIdAsync(request.DepartmentId);
        if (!designation.Any(d => d.Id == request.DesignationId))
            throw new Exception("Invalid Designation for selected Department");

        // 3. Check Email
        var existingUser = await _userRepo.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Email already exists");

        // 4. Generate IDs
        var number = await _employeeRepo.GetNextEmployeeNumberAsync();

        var userId = UserIdGenerator.GenerateUserId("EMP", number);
        var employeeCode = $"EMS{number.ToString("D3")}";

        var user = new User
        {
            UserId = userId,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Employee,
            Status = AccountStatus.Active
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveAsync();

        var employee = new Employee
        {
            EmployeeCode = employeeCode,
            Name = request.Name,
            Phone = request.Phone,
            Address = request.Address,
            DepartmentId = request.DepartmentId,
            DesignationId = request.DesignationId,
            JoiningDate = request.JoiningDate,
            UserId = user.Id
        };

        await _employeeRepo.AddAsync(employee);
        await _employeeRepo.SaveAsync();
    }

    public async Task<PagedResponseDto<EmployeeListResponseDto>> GetPagedAsync(PaginationRequestDto request)
    {
        var (data, total) = await _employeeRepo.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Search
        );

        var items = data.Select(e => new EmployeeListResponseDto
        {
            Id = e.Id,
            EmployeeCode = e.EmployeeCode,
            Name = e.Name,
            Email = e.User.Email,
            Phone = e.Phone,
            Address = e.Address,
            InactiveReason = e.User.InactiveReason,
            Department = e.Department.Code,
            Designation = e.Designation.Name,
            Status = e.User.Status.ToString(),
            DepartmentId = e.DepartmentId,
            DesignationId = e.DesignationId,
            JoiningDate = e.JoiningDate,
        }).ToList();

        return new PagedResponseDto<EmployeeListResponseDto>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total,
            Items = items
        };
    }

    public async Task<EmployeeDetailResponseDto> GetByIdAsync(int id)
    {
        var e = await _employeeRepo.GetByIdAsync(id)
            ?? throw new Exception("Employee not found");

        return new EmployeeDetailResponseDto
        {
            Id = e.Id,
            EmployeeCode = e.EmployeeCode,
            UserId = e.User.UserId,
            Name = e.Name,
            Email = e.User.Email,
            Phone = e.Phone,
            Address = e.Address,
            DepartmentId = e.DepartmentId,
            Department = e.Department.Code,
            DesignationId = e.DesignationId,
            Designation = e.Designation.Name,
            JoiningDate = e.JoiningDate,
            Status = e.User.Status.ToString()
        };
    }

    public async Task UpdateAsync(int id, UpdateEmployeeRequestDto request)
    {
        var employee = await _employeeRepo.GetByIdAsync(id)
            ?? throw new Exception("Employee not found");

        var department = await _departmentRepo.GetByIdAsync(request.DepartmentId)
            ?? throw new Exception("Invalid Department");

        var designations = await _designationRepo.GetByDepartmentIdAsync(request.DepartmentId);

        if (!designations.Any(d => d.Id == request.DesignationId))
            throw new Exception("Invalid Designation");

        employee.Name = request.Name;
        employee.DepartmentId = request.DepartmentId;
        employee.DesignationId = request.DesignationId;
        employee.JoiningDate = request.JoiningDate;
        employee.Phone = request.Phone;
        employee.Address = request.Address;

        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.SaveAsync();
    }

    public async Task ToggleStatusAsync(int employeeId, bool isActive, string? reason)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
    ?? throw new Exception("Employee not found");

        if (employee.User.Role == UserRole.Admin)
            throw new BadRequestException("Admin cannot be deactivated");

        employee.User.Status = isActive
            ? AccountStatus.Active
            : AccountStatus.Inactive;

        employee.User.InactiveReason = isActive ? null : reason;

        await _employeeRepo.SaveAsync();
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        var user = await _userRepo.GetByEmailWithEmployeeAsync(request.Email!)
            ?? throw new Exception("User not found");

        if (request.NewPassword != request.ConfirmPassword)
            throw new Exception("Passwords do not match");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        await _userRepo.UpdateAsync(user);
        await _userRepo.SaveAsync();
    }
    
    public async Task<DashboardSummaryDto> GetAdminSummaryAsync()
    {
        return new DashboardSummaryDto
        {
            ActiveEmployees = await _employeeRepo.GetActiveEmployeeCountAsync(),
            InactiveEmployees = await _employeeRepo.GetInactiveEmployeeCountAsync(),
            PendingLeaves = await _leaveRepo.GetPendingCountAsync(),
            PendingCorrections = await _correctionRepo.GetPendingCountAsync()
        };
    }

    public async Task<List<object>> GetAllCorrectionsAsync()
    {
        return await _correctionRepo.GetAllAsync();
    }
}