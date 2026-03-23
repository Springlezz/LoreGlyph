namespace LoreGlyph.Models
{
    public class Word
    {
        public int WordId { get; set; }
        public string Text { get; set; }
        public string Translation { get; set; }
        public string Transcription { get; set; }
        public int Order { get; set; }
        //public string Symbol {get;set;} in next update
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }
}
