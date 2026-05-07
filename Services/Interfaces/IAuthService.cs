using SmartEMS.API.DTOs.Auth;

namespace SmartEMS.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}