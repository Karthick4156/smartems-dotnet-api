using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Models;
using SmartEMS.API.Repositories.Interfaces;

namespace SmartEMS.API.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task AddAsync(User user)
    {
        _context.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetNextUserNumberAsync()
    {
        var count = await _context.Users
            .CountAsync(u => u.Role == UserRole.Employee);

        return count + 1;
    }

    public async Task<User?> GetByEmailWithEmployeeAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Employee != null)
            .Include(u => u.Employee!)
                .ThenInclude(e => e.Department)
            .Include(u => u.Employee!)
                .ThenInclude(e => e.Designation)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}