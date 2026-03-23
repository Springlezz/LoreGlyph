namespace LoreGlyph.DTOs.Word
{
    public record WordDto
    (
        int WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record CreateWordDto
    (
        int WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record UpdateWordDto
    (
        int WordId,
        string Text,
        string Transcription,
        string Translation,
        int Order
    );

    public record UpdateWordOrderDto(
        int WordId,
        int Order
    );
}