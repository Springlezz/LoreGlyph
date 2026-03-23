using System.Security.Cryptography.X509Certificates;

namespace LoreGlyph.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Login { get; set; }
        public string SecretWordHash { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Language> Languages { get; set; } = new List<Language>();
    }
}
