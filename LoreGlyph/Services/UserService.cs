using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Services.Interfaces;
using LoreGlyph.Repository.Interfaces;

namespace LoreGlyph.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly long _fileSizeLimit;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _fileSizeLimit = configuration.GetValue<long>("FileUpload:FileSizeLimit");
        }

        public async Task<AboutUser?> GetMe(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

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

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            var allowedContentTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            if (avatar == null || avatar.Length == 0)
            {
                throw new InvalidOperationException("Файл не выбран");
            }

            var extension = Path.GetExtension(avatar.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Недопустимый формат файла");
            }

            if (avatar.Length > _fileSizeLimit)
            {
                throw new InvalidOperationException("Лимит по размеру файла");
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

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDirectory, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await avatar.CopyToAsync(stream);

            user.AvatarPath = $"/uploads/avatars/{fileName}";

            await _userRepository.UpdateAsync(user);
        }
    }
}