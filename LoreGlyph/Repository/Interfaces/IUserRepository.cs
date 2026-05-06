using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Data.Entities;
using LoreGlyph.Models;

namespace LoreGlyph.Repository.Interfaces;

public interface IUserRepository
{
    Task DeleteAsync(UserEntity user);
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task<UserEntity?> GetByLoginAsync(string login);
    Task UpdateAsync(UserEntity user);
}