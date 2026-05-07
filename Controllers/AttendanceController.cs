using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Attendance;


[ApiController]
[Route("api/employee/attendance")]
[Authorize(Roles = "Employee")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service)
    {
        _service = service;
    }

    [HttpPost("punch-in")]
    public async Task<IActionResult> PunchIn()
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.PunchInAsync(email);

        return Ok(ApiResponse<string>.SuccessResponse("Punched in"));
    }

    [HttpPost("punch-out")]
    public async Task<IActionResult> PunchOut()
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        await _service.PunchOutAsync(email);

        return Ok(ApiResponse<string>.SuccessResponse("Punched out"));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAttendance()
    {
        var email = User.Identity?.Name ?? throw new Exception("Invalid token");

        var result = await _service.GetMyAttendanceAsync(email);

        return Ok(ApiResponse<List<AttendanceResponseDto>>.SuccessResponse(result));
    }
}