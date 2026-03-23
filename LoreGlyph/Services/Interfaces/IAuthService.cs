using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;

namespace LoreGlyph.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}