using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;

namespace SmartEMS.API.Repositories.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }
}