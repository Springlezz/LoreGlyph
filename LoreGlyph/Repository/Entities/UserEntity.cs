using System.ComponentModel.DataAnnotations.Schema;

namespace LoreGlyph.Repository.Entities
{
    public class UserEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("secret_word_hash")]
        public string SecretWordHash { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }
        [Column("avatar_path")]
        public string? AvatarPath { get; set; }
        public ICollection<LanguageEntity> Languages { get; set; } = new List<LanguageEntity>();
    }
}
