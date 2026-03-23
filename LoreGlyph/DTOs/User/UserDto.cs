namespace LoreGlyph.DTOs.User
{
    public record UserDto
    (
        string UserName,
        string Login
    );

    public record AboutUser
    (
        string UserName,
        string Login,
        DateTime CreatedBy
    );

}