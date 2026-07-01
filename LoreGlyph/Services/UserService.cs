
using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Services.Interfaces;
using LoreGlyph.Repository.Interfaces;

namespace LoreGlyph.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AboutUser?> GetMe(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            return new AboutUser(
                user.Name,
                user.Login,
                user.CreatedAt,
                user.AvatarPath
            );
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }
            
            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> ResetForgottenPasswordAsync(ResetForgottenPasswordDto dto)
        {
            var user = await _userRepository.GetByLoginAsync(dto.Login);

            if (user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.SecretWord, user.SecretWordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task UploadAvatarAsync(Guid userId, IFormFile avatar)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(user.AvatarPath))
            {
                var oldFilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    user.AvatarPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)
                );

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            var uploadDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "avatars");

            Directory.CreateDirectory(uploadDirectory);

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(avatar.FileName)}";

            var filePath = Path.Combine(uploadDirectory, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await avatar.CopyToAsync(stream);

            user.AvatarPath = $"/uploads/avatars/{fileName}";

            await _userRepository.UpdateAsync(user);
        }
    }
}
