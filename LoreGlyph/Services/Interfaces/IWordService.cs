using LoreGlyph.DTOs.User;
using LoreGlyph.DTOs.Word;

namespace LoreGlyph.Services.Interfaces
{
    public interface IWordService
    {
        Task<IEnumerable<WordDto>> GetAllAsync(Guid languageId, Guid userId);
        Task<WordDto> CreateAsync(CreateWordDto dto, Guid languageId, Guid userId);
        Task<bool> DeleteAsync(Guid wordId, Guid userId);
        Task<bool> UpdateAsync(Guid wordId, UpdateWordDto dto);
        Task<bool> UpdateOrderAsync (IList<UpdateWordOrderDto> dto, Guid userId, Guid languageId);
    }
}
