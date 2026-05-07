using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.DTOs.Correction;

namespace SmartEMS.API.Controllers;

[ApiController]
[Route("api/admin/corrections")]
[Authorize(Roles = "Admin")]
public class AdminCorrectionController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AdminCorrectionController(IAttendanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllCorrectionsAsync();
        return Ok(ApiResponse<List<CorrectionResponseDto>>.SuccessResponse(data));
    }

    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _service.ApproveCorrectionAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse("Approved"));
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        await _service.RejectCorrectionAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse("Rejected"));
    }
}