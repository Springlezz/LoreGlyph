namespace LoreGlyph.Models
{
    public class Language
    {
        public int LanguageId {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        
        public IList<Word> Words { get; set; } = new List<Word>();
    }
}
