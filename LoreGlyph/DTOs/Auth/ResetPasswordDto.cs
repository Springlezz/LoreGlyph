namespace LoreGlyph.DTOs.Auth
{
    public record ResetPasswordDto
    (
        string Login,
        string OldPassword,
        string NewPassword
    );

    public record ResetForgottenPasswordDto
    (
        string Login,
        string NewPassword,
        string SecretWord
    );
}


