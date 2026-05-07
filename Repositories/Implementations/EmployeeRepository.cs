using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Models;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNextEmployeeNumberAsync()
    {
        var count = await _context.Users
            .CountAsync(u => u.Role == UserRole.Employee);

        return count + 1;
    }

    public async Task<(List<Employee>, int)> GetPagedAsync(int page, int pageSize, string? search)
    {
        var query = _context.Employees
            .Include(e => e.User)
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.Name.Contains(search) ||
                e.User.Email.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.User)
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<int> GetActiveEmployeeCountAsync()
    {
        return await _context.Employees
            .Include(e => e.User)
            .CountAsync(e => e.User.Status == AccountStatus.Active);
    }

    public async Task<int> GetInactiveEmployeeCountAsync()
    {
        return await _context.Employees
            .Include(e => e.User)
            .CountAsync(e => e.User.Status == AccountStatus.Inactive);
    }
}