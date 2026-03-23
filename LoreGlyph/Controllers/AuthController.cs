using LoreGlyph.DTOs.Auth;
using LoreGlyph.DTOs.User;
using LoreGlyph.Services;
using LoreGlyph.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoreGlyph.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult?> RegisterAsync(RegisterDto dto)
        {
            try
            {
                var user = await _authService.RegisterAsync(dto);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult?> LoginAsync(LoginDto dto)
        {
            var authDto = await _authService.LoginAsync(dto);

            if (authDto == null)
            {
                return BadRequest("Неверный логин или пароль");
            }

            return Ok(authDto);
        }

        //пароль, меняющийся в аккаунте! Перемещать на данный момент в user-зависимостях не буду
        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var authDto = await _authService.ResetPasswordAsync(dto);

            if (authDto == null)
            {
                return BadRequest("Старый пароль неверный. Попробуйте еще раз");
            }

            return Ok(authDto);
        }
    }
}
