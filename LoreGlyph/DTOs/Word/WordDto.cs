namespace LoreGlyph.DTOs.Word
{
    public record WordDto
    (
        Guid WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record CreateWordDto
    (
        Guid WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record UpdateWordDto
    (
        Guid WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record UpdateWordOrderDto(
        Guid WordId,
        int Order
    );

    public record ShareLanguageDto(
        string AuthorName,
        string? AuthorAvatarUrl,
        string LanguageName,
        IEnumerable<WordDto> Words
    );
}