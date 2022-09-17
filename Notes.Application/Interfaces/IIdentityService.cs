using Notes.Application.Common.Dtos.IdentityDtos;

namespace Notes.Application.Interfaces;

public interface IIdentityService
{
    Task<AuthResponseDto> LoginAsync(LoginDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto tokenDto);
    Task LogoutAsync(LogoutDto request);
    //Task SignOutAsync(LogoutDto request);
}
