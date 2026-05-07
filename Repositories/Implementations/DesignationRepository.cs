using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Models;

public class DesignationRepository : IDesignationRepository
{
    private readonly AppDbContext _context;

    public DesignationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Designation>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Designations
            .Where(d => d.DepartmentId == departmentId)
            .ToListAsync();
    }
}