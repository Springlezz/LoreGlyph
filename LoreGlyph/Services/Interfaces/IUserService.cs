using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;

namespace LoreGlyph.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ResetForgottenPasswordAsync(ResetForgottenPasswordDto dto);
        Task<AboutUser?> GetMe(int userId);
        Task<bool> DeleteAsync(int userId);
        
    }
}
