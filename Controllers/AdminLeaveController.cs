using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Leave;

[ApiController]
[Route("api/admin/leaves")]
[Authorize(Roles = "Admin")]
public class AdminLeaveController : ControllerBase
{
    private readonly ILeaveService _service;

    public AdminLeaveController(ILeaveService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<List<LeaveResponseDto>>.SuccessResponse(data));
    }

    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _service.ApproveAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse("Approved"));
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        await _service.RejectAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse("Rejected"));
    }
}