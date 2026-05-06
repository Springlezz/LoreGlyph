using BCrypt.Net;
using LoreGlyph.Data;
using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Services.Interfaces;
using LoreGlyph.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.Repository;

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
                user.UserName,
                user.Login,
                user.CreatedAt
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
    }
}
