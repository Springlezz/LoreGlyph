using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Models;
using LoreGlyph.Services;
using LoreGlyph.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoreGlyph.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult?> GetMe(int userId)
        {
            try
            {
                var result = await _userService.GetMe(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-forgotten-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetForgottenPasswordAsync(ResetForgottenPasswordDto dto)
        {
            var authDto = await _userService.ResetForgottenPasswordAsync(dto);

            if (authDto == null)
            {
                return BadRequest("Кодовое слово неверное");
            }

            return Ok(authDto);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            try
            {
                var result = await _userService.DeleteAsync(userId);

                if (!result)
                {
                    return NotFound("Пользователь не найден");
                }
                return Ok("Удалено");
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить пользователя");
            }
        }
    }
}