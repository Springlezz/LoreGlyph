using LoreGlyph.DTOs.Language;
using LoreGlyph.Models;

namespace LoreGlyph.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageDto>> GetAllAsync(Guid userId);
        Task<LanguageDto> CreateAsync(CreateLanguageDto dto, Guid userId);
        Task<bool> UpdateAsync(Guid languageId, UpdateLanguageDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid languageId, Guid userId);
    }
}
