using LoreGlyph.Repository.Entities;

namespace LoreGlyph.Repository.Interfaces;

public interface IWordRepository
{
    Task<List<WordEntity>> GetAllByLanguageIdAsync(Guid languageId);
    Task<WordEntity?> GetByIdAsync(Guid wordId);
    Task AddAsync(WordEntity word);
    Task UpdateAsync(WordEntity word);
    Task DeleteAsync(WordEntity word);
}