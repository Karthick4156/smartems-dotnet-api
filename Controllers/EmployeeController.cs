using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.DTOs.Employee;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Correction;

[ApiController]
[Route("api/employee")]
[Authorize(Roles = "Employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeSelfService _service;

    public EmployeeController(IEmployeeSelfService service)
    {
        _service = service;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        var result = await _service.GetProfileAsync(email);
        return Ok(ApiResponse<EmployeeProfileResponseDto>.SuccessResponse(result));
    }

    [HttpPut("profile-edit")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequestDto request)
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.UpdateProfileAsync(email, request);
        return Ok(ApiResponse<string>.SuccessResponse("Profile updated"));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.ChangePasswordAsync(email, request);
        return Ok(ApiResponse<string>.SuccessResponse("Password changed"));
    }

    [HttpPost("correction")]
    public async Task<IActionResult> RequestCorrection(
        [FromBody] CreateCorrectionRequestDto dto) // ✅ FIX
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.RequestCorrectionAsync(email, dto.Date, dto.Reason);

        return Ok(ApiResponse<string>.SuccessResponse("Request submitted"));
    }

    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetDashboard()
    {
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            throw new Exception("Invalid user");

        var data = await _service.GetDashboardAsync(email);

        return Ok(ApiResponse<object>.SuccessResponse(data));
    }
}