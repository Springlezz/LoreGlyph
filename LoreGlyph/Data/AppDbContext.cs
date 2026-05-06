using LoreGlyph.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LoreGlyph.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<WordEntity> Words { get; set; } = null!;
        public DbSet<LanguageEntity> Languages { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LanguageEntity>()
                .HasMany(l => l.Words)
                .WithOne(w => w.Language)
                .HasForeignKey(w => w.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}