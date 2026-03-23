using LoreGlyph.DTOs.Language;

namespace LoreGlyph.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageDto>> GetAllAsync(int languageId);
        //Task<LanguageDto> GetByIdAsync(int languageId, int userId);
        Task<LanguageDto> CreateAsync(CreateLanguageDto dto, int userId);
        Task<bool> UpdateAsync(int languageId, UpdateLanguageDto dto);
        Task<bool> DeleteAsync(int languageId, int userId);
    }
}
