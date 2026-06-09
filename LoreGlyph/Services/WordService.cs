using LoreGlyph.DTOs.Word;
using LoreGlyph.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.Repository.Interfaces;
using LoreGlyph.Repository.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LoreGlyph.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly ILanguageRepository _languageRepository;

        public WordService(IWordRepository wordRepository, ILanguageRepository languageRepository)
        {
            _wordRepository = wordRepository;
            _languageRepository = languageRepository;
        }

        public async Task<IEnumerable<WordDto>> GetAllAsync(Guid userId, Guid languageId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language == null || language.UserId != userId)
            {
                return Enumerable.Empty<WordDto>();
            }
            
            var words = await _wordRepository.GetAllByLanguageIdAsync(languageId);

            return words
                .OrderBy(w => w.Order)
                .Select(w => new WordDto(w.Id, w.Text, w.Transcription, w.Translation, w.Order));
        }

        
        public async Task<WordDto> CreateAsync(CreateWordDto dto, Guid languageId, Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language == null)
            {
                throw new Exception("Язык не найден");
            }

            var word = new WordEntity
            {
                LanguageId = languageId,
                Text = dto.Text,
                Transcription = dto.Transcription,
                Translation = dto.Translation,
                Order = dto.Order
            };
            
            await _wordRepository.AddAsync(word);

            return new WordDto(
                    word.Id,
                    word.Text,
                    word.Transcription,
                    word.Translation,
                    word.Order
                );
        }

        public async Task<bool> DeleteAsync(Guid wordId, Guid userId)
        {
            var word = await _wordRepository.GetByIdAsync(wordId);
            
            if (word == null)
            {
                return false;
            }
            
            var language = await _languageRepository.GetByIdAsync(word.LanguageId);

            if (language == null || language.UserId != userId)
            {
                return false;
            }
            
            await  _wordRepository.DeleteAsync(word);
            return true;
        }

        public async Task<bool> UpdateAsync(Guid wordId, UpdateWordDto dto)
        {
            var word = await _wordRepository.GetByIdAsync(wordId);

            if (word == null)
            {
                return false;
            }
            
            var language = await _languageRepository.GetByIdAsync(word.LanguageId);

            if (language == null)
            {
                return false;
            }

            word.Text = dto.Text;
            word.Transcription = dto.Transcription;
            word.Translation = dto.Translation;

            await _wordRepository.UpdateAsync(word);
            return true;
        }

        public async Task<bool> UpdateOrderAsync(IList<UpdateWordOrderDto> dto, Guid languageId, Guid userId)
        {
            var language = await _languageRepository.GetByIdAsync(languageId);
            
            if (language == null || language.UserId != userId)
            {
                return false;
            }
            
            var words = await _wordRepository.GetAllByLanguageIdAsync(languageId);
            
            if (words.Count == 0)
            {
                return false;
            }

            foreach (var item in dto)
            {
                var word = words.FirstOrDefault(w => w.Id == item.WordId);
                if (word != null)
                {
                    word.Order = item.Order;
                }
            }
            
            foreach (var word in words)
            {
                await _wordRepository.UpdateAsync(word);
            }
            
            return true;
        }

        public async Task<IEnumerable<WordDto>> GetSharedWordsAsync(string token)
        {
            var language = await _languageRepository.GetByShareTokenAsync(token);

            if (language == null || !language.IsPublic)
            {
                return null;
            }

            var words = await _wordRepository.GetAllByLanguageIdAsync(language.Id);

            return words
                .OrderBy(w => w.Order)
                .Select(w => new WordDto(
                    w.Id,
                    w.Text,
                    w.Transcription,
                    w.Translation,
                    w.Order));
        }
    }
}
