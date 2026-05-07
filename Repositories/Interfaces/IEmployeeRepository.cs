using SmartEMS.API.Models;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task SaveAsync();

    Task<int> GetNextEmployeeNumberAsync();
    Task<(List<Employee> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search);
    Task<Employee?> GetByIdAsync(int id);
    Task<int> GetActiveEmployeeCountAsync();
    Task<int> GetInactiveEmployeeCountAsync();
}