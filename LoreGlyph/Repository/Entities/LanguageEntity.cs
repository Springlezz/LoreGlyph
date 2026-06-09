using System.ComponentModel.DataAnnotations.Schema;

namespace LoreGlyph.Repository.Entities
{
    public class LanguageEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }
        
        [Column("link")]
        public bool IsPublic { get; set; }
        public string? ShareToken { get; set; }

        public IList<WordEntity> Words { get; set; } = new List<WordEntity>();
    }
}
