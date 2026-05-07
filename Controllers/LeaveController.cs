using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Leave;


[ApiController]
[Route("api/employee/leaves")]
[Authorize(Roles = "Employee")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveService _service;

    public LeaveController(ILeaveService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Apply(CreateLeaveRequestDto request)
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.ApplyLeaveAsync(email, request);

        return Ok(ApiResponse<string>.SuccessResponse("Leave applied"));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyLeaves()
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        var result = await _service.GetMyLeavesAsync(email);

        return Ok(ApiResponse<List<LeaveResponseDto>>.SuccessResponse(result));
    }
}