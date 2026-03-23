using LoreGlyph.Data;
using LoreGlyph.DTOs.Language;
using LoreGlyph.Models;
using LoreGlyph.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoreGlyph.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;
        public LanguageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LanguageDto>> GetAllAsync(int userId)
        {
            return await _context.Languages
                .Where(l => l.UserId == userId)
                .Select(language => new LanguageDto(
                    language.LanguageId,
                    language.Name,
                    language.Description
                    ))
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int languageId, UpdateLanguageDto dto)
        {
            var language = await _context.Languages.FindAsync(languageId);

            if (language == null)
            {
                return false;
            }

            language.Name = dto.Name;
            language.Description = dto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int languageId, int userId)
        {
            var language = await _context.Languages
                .FirstOrDefaultAsync(l => l.LanguageId == languageId && l.UserId == userId);
            if (language == null)
            {
                return false;
            }

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<LanguageDto> GetByIdAsync(int languageId)
        //{
        //    var language = await _context.Languages
        //        .FirstOrDefaultAsync(l => l.LanguageId == languageId && l.UserId == userId);

        //    if (language == null) return null;

        //    return new LanguageDto(
        //        language.LanguageId,
        //        language.Name,
        //        language.Description
        //    );
        //}

        public async Task<LanguageDto> CreateAsync(CreateLanguageDto dto, int userId)
        {
            var language = new Language
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return new LanguageDto(
                language.LanguageId,
                language.Name,
                language.Description
            );
        }
    }
}
