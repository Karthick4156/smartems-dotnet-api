using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.Helpers;
using SmartEMS.API.DTOs.Designation;

[ApiController]
[Route("api/designations")]
public class DesignationController : ControllerBase
{
    private readonly IDesignationService _service;

    public DesignationController(IDesignationService service)
    {
        _service = service;
    }

    [HttpGet("by-department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        var data = await _service.GetByDepartmentIdAsync(departmentId);
        return Ok(ApiResponse<List<DesignationResponseDto>>.SuccessResponse(data));
    }
}