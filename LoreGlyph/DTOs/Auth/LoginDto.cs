namespace LoreGlyph.DTOs.Auth
{
    public record LoginDto
        (
            string Login,
            string Password
        );

    public record AuthResponseDto(
        string Token,
        string UserName,
        string Login
    );
}
