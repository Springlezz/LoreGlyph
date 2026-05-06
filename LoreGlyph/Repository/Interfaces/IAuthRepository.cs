using LoreGlyph.Repository.Entities;

namespace LoreGlyph.Repository.Interfaces;

public interface IAuthRepository
{
    Task<bool> LoginExistsAsync(string login);
    
    Task<UserEntity?> GetByLoginAsync(string login);
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
}