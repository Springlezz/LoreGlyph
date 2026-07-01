using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;

namespace LoreGlyph.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ResetForgottenPasswordAsync(ResetForgottenPasswordDto dto);
        Task<AboutUser?> GetMe(Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task UploadAvatarAsync(Guid userId, IFormFile avatar);
    }
}
