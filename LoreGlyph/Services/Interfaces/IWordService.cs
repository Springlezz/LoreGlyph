using LoreGlyph.DTOs.User;
using LoreGlyph.DTOs.Word;

namespace LoreGlyph.Services.Interfaces
{
    public interface IWordService
    {
        Task<IEnumerable<WordDto>> GetAllAsync(int languageId);
        Task<WordDto> CreateAsync(CreateWordDto dto, int languageId, int userId);
        Task<bool> DeleteAsync(int wordId, int userId);
        Task<bool> UpdateAsync(int wordId, UpdateWordDto dto);
        Task<bool> UpdateOrderAsync (IList<UpdateWordOrderDto> dto, int userId, int languageId);
    }
}
