using System.ComponentModel.DataAnnotations.Schema;

namespace LoreGlyph.Repository.Entities
{
    public class WordEntity : BaseEntity
    {
        [Column("text")]
        public string Text { get; set; }
        [Column("translation")]
        public string Translation { get; set; }
        [Column("transcription")]
        public string Transcription { get; set; }
        [Column("order")]
        public int Order { get; set; }
        [Column("language_id")]
        public Guid LanguageId { get; set; }
        public virtual LanguageEntity? Language { get; set; }
    }
}
