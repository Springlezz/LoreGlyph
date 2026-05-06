using LoreGlyph.Data.Entities;

namespace LoreGlyph.Models
{
    public class WordEntity : BaseEntity
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public string Transcription { get; set; }
        public int Order { get; set; }
        public Guid LanguageId { get; set; }
        public LanguageEntity? Language { get; set; }
        //public string Symbol {get;set;} in next update
    }
}
