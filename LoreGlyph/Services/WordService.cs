using LoreGlyph.DTOs.Word;
using LoreGlyph.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.Models;
using LoreGlyph.Data;

namespace LoreGlyph.Services
{
    public class WordService : IWordService
    {
        private readonly AppDbContext _context;

        public WordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WordDto>> GetAllAsync(int userId)
        {
            return await _context.Words
                .Where (word => word.Language.UserId == userId)
                .OrderBy (word => word.Order)
                .Select(word => new WordDto(
                                      word.WordId,
                                      word.Text,
                                      word.Transcription,
                                      word.Translation,
                                      word.Order
                                      ))
                .ToListAsync();
        }

        
        public async Task<WordDto> CreateAsync(CreateWordDto dto, int languageId, int userId)
        {
            var language = await _context.Languages
                .FirstOrDefaultAsync(languages => languages.LanguageId == languageId && languages.UserId == userId);

            if (language == null)
            {
                throw new Exception("Язык не найден");
            }

            var word = new Word
            {
                LanguageId = languageId,
                Text = dto.Text,
                Transcription = dto.Transcription,
                Translation = dto.Translation,
                Order = dto.Order
            };

            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return new WordDto(
                    word.WordId,
                    word.Text,
                    word.Transcription,
                    word.Translation,
                    word.Order
                );
        }

        public async Task<bool> DeleteAsync(int wordId, int userId)
        {
            var word = await _context.Words
                .FirstOrDefaultAsync(w => w.WordId == wordId && w.Language.UserId == userId);
            if (word == null)
            {
                return false;
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int wordId, UpdateWordDto dto)
        {
            var word = await _context.Words.FindAsync(wordId);

            if (word == null)
            {
                return false;
            }

            word.Text = dto.Text;
            word.Transcription = dto.Transcription;
            word.Translation = dto.Translation;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderAsync(IList<UpdateWordOrderDto> dto, int languageId, int userId)
        {
            var words = await _context.Words
                .Where(word => word.LanguageId == languageId && word.Language.UserId == userId)
                .ToListAsync();
            
            if (words.Count == 0)
            {
                return false;
            }

            foreach (var item in dto)
            {
                var word = words.FirstOrDefault(w => w.WordId == item.WordId);
                if (word != null)
                {
                    word.Order = item.Order;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
