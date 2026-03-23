namespace LoreGlyph.DTOs.Auth
{
    public record RegisterDto
    (
        string Login,
        string Password,
        string SecretWord,
        string UserName
    );

}
