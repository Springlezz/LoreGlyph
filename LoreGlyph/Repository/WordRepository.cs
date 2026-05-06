using LoreGlyph.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.Models;
using LoreGlyph.Data;

namespace LoreGlyph.Repository;

public class WordRepository : IWordRepository
{
    private readonly AppDbContext _context;
    
    public WordRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<WordEntity>> GetAllByLanguageIdAsync(Guid languageId)
    {
        return await  _context.Words
            .Where(w => w.LanguageId == languageId)
            .ToListAsync();
    }

    public async Task<WordEntity?> GetByIdAsync(Guid wordId)
    {
        return await _context.Words.FindAsync(wordId);
    }

    public async Task AddAsync(WordEntity word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(WordEntity word)
    {
        _context.Words.Update(word);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WordEntity word)
    {
        _context.Words.Remove(word);
        await _context.SaveChangesAsync();
    }
}