using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.DTOs.Employee;
using SmartEMS.API.DTOs.Common;
using SmartEMS.API.Helpers;
using SmartEMS.API.DTOs.Dashboard;


[ApiController]
[Route("api/admin/employees")]
[Authorize(Roles = "Admin")]
public class AdminEmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public AdminEmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeRequestDto request)
    {
        await _service.CreateEmployeeAsync(request);
        return Ok(ApiResponse<string>.SuccessResponse("Employee created successfully"));
    }
  
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDto request)
    {
        var result = await _service.GetPagedAsync(request);
        return Ok(ApiResponse<PagedResponseDto<EmployeeListResponseDto>>
            .SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<EmployeeDetailResponseDto>.SuccessResponse(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEmployeeRequestDto request)
    {
        await _service.UpdateAsync(id, request);
        return Ok(ApiResponse<string>.SuccessResponse("Updated successfully"));
    }

    [HttpPatch("{userId}/status")]
    public async Task<IActionResult> ToggleStatus(int userId, [FromQuery] ToggleStatusDto dto)
    {
        await _service.ToggleStatusAsync(userId, dto.IsActive, dto.Reason!);
        return Ok(ApiResponse<string>.SuccessResponse("Status updated"));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
    {
        await _service.ResetPasswordAsync(request);
        return Ok(ApiResponse<string>.SuccessResponse("Password reset successful"));
    }

    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var result = await _service.GetAdminSummaryAsync();
            return Ok(ApiResponse<DashboardSummaryDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }
}