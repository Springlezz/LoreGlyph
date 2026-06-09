namespace LoreGlyph.DTOs.Language
{
    public record UpdateLanguageDto(
        string Name,
        string Description
    );

    public record LanguageDto(
        Guid LanguageId,
        string Name,
        string Description
    );

    public record CreateLanguageDto
    (
        string Name,
        string Description
    );
    
    public record LanguageShareDto(
        bool IsPublic,
        string? ShareToken
    );
}
