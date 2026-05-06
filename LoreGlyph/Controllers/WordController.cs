using LoreGlyph.DTOs.Language;
using LoreGlyph.DTOs.Word;
using LoreGlyph.Services;
using LoreGlyph.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoreGlyph.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordController : Controller
    {
        private readonly IWordService _wordService;
        public WordController(IWordService wordService)
        {
           _wordService = wordService;
        }

        [HttpGet("{languageId}")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync(Guid languageId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var words = await _wordService.GetAllAsync(userId, languageId);
                return Ok(words);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{languageId}")]
        [Authorize]
        public async Task<IActionResult> Create(Guid languageId, CreateWordDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var word = await _wordService.CreateAsync(dto, languageId, userId);
                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{wordId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid wordId, UpdateWordDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _wordService.UpdateAsync(wordId, dto);

                if (!result)
                {
                    return NotFound("Слово, к которому вы обращаетесь, не найдено");
                }

                return Ok("Слово изменено");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{wordId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid wordId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _wordService.DeleteAsync(wordId, userId);

                if (!result)
                {
                    return NotFound("Слово не найдено");
                }
                return Ok("Удалено");
            }
            catch (Exception ex)
            {
                return BadRequest($"Не удалось удалить {ex.Message}");
            }
        }

        [HttpPut("update-word-order")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderAsync([FromBody] IList<UpdateWordOrderDto> dto, Guid languageId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (dto == null)
                {
                    return BadRequest("Список слов не может быть пустым");
                }

                var result = await _wordService.UpdateOrderAsync(dto, languageId, userId);

                if (!result)
                {
                    return NotFound("Язык или слова не найдены");
                }

                return Ok("Порядок слов успешно обновлен");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обновлении порядка слов: {ex.Message}");
            }
        }
    }
}