using SmartEMS.API.DTOs.Leave;


namespace SmartEMS.API.Services.Interfaces;

public interface ILeaveService
{
    Task ApplyLeaveAsync(string email, CreateLeaveRequestDto request);
    Task<List<LeaveResponseDto>> GetMyLeavesAsync(string email);
    Task<List<LeaveResponseDto>> GetAllAsync();
    Task ApproveAsync(int id);
    Task RejectAsync(int id);
}