using LoreGlyph.Repository.Entities;

namespace LoreGlyph.Repository.Interfaces;

public interface IUserRepository
{
    Task DeleteAsync(UserEntity user);
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task<UserEntity?> GetByLoginAsync(string login);
    Task UpdateAsync(UserEntity user);
}