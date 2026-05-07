using SmartEMS.API.Models;

namespace SmartEMS.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);

    Task UpdateAsync(User user);
    Task SaveAsync();
    Task<int> GetNextUserNumberAsync();
    Task<User?> GetByEmailWithEmployeeAsync(string email);
}