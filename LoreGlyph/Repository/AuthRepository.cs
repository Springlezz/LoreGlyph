using Microsoft.EntityFrameworkCore;
using LoreGlyph.Repository.Entities;


namespace LoreGlyph.Repository.Interfaces;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> LoginExistsAsync(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task<UserEntity?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }


    public async Task AddAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}