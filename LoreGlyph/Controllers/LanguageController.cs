using LoreGlyph.DTOs.Language;
using LoreGlyph.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoreGlyph.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {        
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var languages = await _languageService.GetAllAsync(userId);
                return Ok(languages);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{languageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int languageId, UpdateLanguageDto dto)
        {  
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _languageService.UpdateAsync(languageId, dto);

                if (!result)
                {
                    return NotFound("Язык, к которому вы обращаетесь, не найден");
                }

                return Ok("Язык изменен");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(CreateLanguageDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var languages = await _languageService.CreateAsync(dto, userId);
                return Ok("Создано");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{languageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int languageId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _languageService.DeleteAsync(languageId, userId);

                if (!result)
                {
                    return NotFound("Язык не найден");
                }
                return Ok("Удалено");
            }
            catch (Exception ex) 
            {
                return BadRequest($"Не удалось удалить {languageId}{ex.Message}");
            }
        }
    }
}
