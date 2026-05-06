using LoreGlyph.DTOs.Auth;
using LoreGlyph.Data;
using LoreGlyph.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using LoreGlyph.DTOs.User;
using LoreGlyph.Data.Entities;
using LoreGlyph.Models;
using Microsoft.EntityFrameworkCore;

namespace LoreGlyph.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<UserEntity?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(UserEntity user)
    {
        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }
}