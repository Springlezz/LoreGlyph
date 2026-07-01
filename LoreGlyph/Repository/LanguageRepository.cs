using LoreGlyph.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.Repository.Entities;


namespace LoreGlyph.Repository;

public class LanguageRepository : ILanguageRepository
{
    private readonly AppDbContext _context;
    
    public LanguageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LanguageEntity>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Languages
            .Where(l => l.UserId == userId)
            .ToListAsync();
    }

    public async Task<LanguageEntity?> GetByIdAsync(Guid languageId)
    {
        return await _context.Languages.FindAsync(languageId);
    }

    public async Task UpdateAsync(LanguageEntity language)
    {
        _context.Languages.Update(language);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(LanguageEntity language)
    {
        _context.Languages.Add(language);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(LanguageEntity language)
    {
        _context.Languages.Remove(language);
        await _context.SaveChangesAsync();
    }

    public async Task<LanguageEntity?> GetByShareTokenAsync(string token)
    {
        return await _context.Languages
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.ShareToken == token && l.IsPublic);
    }
}