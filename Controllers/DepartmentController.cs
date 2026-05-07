using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.DTOs.Department;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;

namespace SmartEMS.API.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<List<DepartmentResponseDto>>.SuccessResponse(data));
    }
}