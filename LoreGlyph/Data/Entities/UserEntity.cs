using System.Security.Cryptography.X509Certificates;
using LoreGlyph.Data.Entities;

namespace LoreGlyph.Models
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string Login { get; set; }
        public string SecretWordHash { get; set; }
        public string PasswordHash { get; set; }
        public string AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<LanguageEntity> Languages { get; set; } = new List<LanguageEntity>();
    }
}
