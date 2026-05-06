using LoreGlyph.Data;
using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Models;
using LoreGlyph.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoreGlyph.Repository.Interfaces;

namespace LoreGlyph.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
        {
            var exists = await _authRepository.LoginExistsAsync(dto.Login);

            if (exists)
            {
                return null;
            }

            var user = new UserEntity
            {
                UserName = dto.UserName,
                Login = dto.Login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                SecretWordHash = BCrypt.Net.BCrypt.HashPassword(dto.SecretWord),
                CreatedAt = DateTime.UtcNow
            };

            await _authRepository.AddAsync(user);

            return new UserDto(
                user.UserName,
                user.Login
                ); 
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.GetByLoginAsync(dto.Login);

            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            var claims = new[ ]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto(
                tokenString,
                user.UserName,
                user.Login
            );
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _authRepository.GetByLoginAsync(dto.Login);

            if (user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _authRepository.UpdateAsync(user);
            return true;
        }
    }
}