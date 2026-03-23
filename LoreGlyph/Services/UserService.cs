using BCrypt.Net;
using LoreGlyph.Data;
using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Services.Interfaces;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace LoreGlyph.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AboutUser?> GetMe(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return new AboutUser(
                user.UserName,
                user.Login,
                user.CreatedAt
                );
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }
               

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetForgottenPasswordAsync(ResetForgottenPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == dto.Login);

            if (user == null)
            {
                return false;
            }


            if (!BCrypt.Net.BCrypt.Verify(dto.SecretWord, user.SecretWordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
