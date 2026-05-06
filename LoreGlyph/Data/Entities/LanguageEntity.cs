using LoreGlyph.Data.Entities;

namespace LoreGlyph.Models
{
    public class LanguageEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public UserEntity UserEntity { get; set; }
        
        public IList<WordEntity> Words { get; set; } = new List<WordEntity>();
    }
}
