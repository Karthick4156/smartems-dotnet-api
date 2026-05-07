using Microsoft.AspNetCore.Mvc;
using SmartEMS.API.DTOs.Auth;
using SmartEMS.API.Helpers;
using SmartEMS.API.Services.Interfaces;

namespace SmartEMS.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result));
    }
}