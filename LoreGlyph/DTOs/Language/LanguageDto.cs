namespace LoreGlyph.DTOs.Language
{
    public record UpdateLanguageDto(
        int LanguageId,
        string Name,
        string Description
    );

    public record LanguageDto(
        int LanguageId,
        string Name,
        string Description
    );

    public record CreateLanguageDto
    (
        int LanguageId,
        string Name,
        string Description
    );

}
