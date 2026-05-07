using SmartEMS.API.DTOs.Employee;
using SmartEMS.API.DTOs.Common;
using SmartEMS.API.DTOs.Dashboard;
using SmartEMS.API.Models;
public interface IEmployeeService
{
    Task CreateEmployeeAsync(CreateEmployeeRequestDto request);

    Task<PagedResponseDto<EmployeeListResponseDto>> GetPagedAsync(PaginationRequestDto request);

    Task<EmployeeDetailResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateEmployeeRequestDto request);
    Task ToggleStatusAsync(int userId, bool isActive, string reason);
    Task ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<DashboardSummaryDto> GetAdminSummaryAsync();
    Task<List<object>> GetAllCorrectionsAsync();

}