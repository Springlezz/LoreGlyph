using LoreGlyph.Repository.Entities;

namespace LoreGlyph.Repository.Interfaces;

public interface ILanguageRepository
{
    Task<List<LanguageEntity>> GetAllByUserIdAsync(Guid userId);
    Task<LanguageEntity?> GetByIdAsync(Guid languageId);
    Task AddAsync(LanguageEntity language);
    Task UpdateAsync(LanguageEntity language);
    Task DeleteAsync(LanguageEntity language);
    Task<LanguageEntity?> GetByShareTokenAsync(string token);
}