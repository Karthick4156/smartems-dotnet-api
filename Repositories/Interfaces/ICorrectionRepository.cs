namespace SmartEMS.API.Repositories.Interfaces;
public interface ICorrectionRepository
{
    Task<int> GetPendingCountAsync();
    Task<List<object>> GetAllAsync();
}