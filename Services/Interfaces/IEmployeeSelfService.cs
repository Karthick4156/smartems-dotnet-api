using SmartEMS.API.DTOs.Employee;

public interface IEmployeeSelfService
{
    Task<EmployeeProfileResponseDto> GetProfileAsync(string email);
    Task UpdateProfileAsync(string email, UpdateProfileRequestDto request);
    Task ChangePasswordAsync(string email, ChangePasswordRequestDto request);
    Task RequestCorrectionAsync(string email, DateTime date, string reason);
    Task<EmployeeDashboardDto> GetDashboardAsync(string email);

}