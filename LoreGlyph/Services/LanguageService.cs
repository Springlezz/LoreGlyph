using LoreGlyph.DTOs.Language;
using LoreGlyph.Helpers;
using LoreGlyph.Repository.Entities;
using LoreGlyph.Repository.Interfaces;
using LoreGlyph.Services.Interfaces;

namespace LoreGlyph.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<IEnumerable<LanguageDto>> GetAllAsync(Guid userId)
        {
            var languages = await _languageRepository.GetAllByUserIdAsync(userId);

            return languages.Select(language => new LanguageDto(
                language.Id,
                language.Name,
                language.Description
            ));
        }

        public async Task<bool> UpdateAsync(Guid languageId, UpdateLanguageDto dto, Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language == null || language.UserId != userId)
            {
                return false;
            }

            language.Name = dto.Name;
            language.Description = dto.Description;

            await _languageRepository.UpdateAsync(language);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid languageId, Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);
            
            if (language == null || language.UserId != userId)
            {
                return false;
            }
            
            await _languageRepository.DeleteAsync(language);
            return true;
        }
        
        public async Task<LanguageDto> CreateAsync(CreateLanguageDto dto, Guid userId)
        {
            var language = new LanguageEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            await _languageRepository.AddAsync(language);

            return new LanguageDto(
                language.Id,
                language.Name,
                language.Description
            );
        }

        public async Task<string> ShareLanguageAsync(Guid languageId, Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language == null || language.UserId != userId)
            {
                return string.Empty;
            }

            language.IsPublic = true;

            if (string.IsNullOrEmpty(language.ShareToken))
            {
                language.ShareToken = LinkGeneration.GenerateToken();
            }
            
            await _languageRepository.UpdateAsync(language);
            return language.ShareToken;
        }

        public async Task<bool> UnshareLanguageAsync(Guid languageId, Guid userId)
        {
            var language =  await _languageRepository.GetByIdAsync(languageId);
            
            if (language == null || language.UserId != userId)
            {
                return false;
            }
            
            language.IsPublic = false;
            
            await _languageRepository.UpdateAsync(language);
            return true;
        }
        
        public async Task<LanguageShareDto?> GetShareInfoAsync(
            Guid languageId,
            Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language == null || language.UserId != userId)
            {
                return null;
            }

            return new LanguageShareDto(
                language.IsPublic,
                language.ShareToken
            );
        }
    }
}
