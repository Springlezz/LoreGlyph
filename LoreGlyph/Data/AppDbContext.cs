using LoreGlyph.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LoreGlyph.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Word> Words { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .HasMany(l => l.Words)
                .WithOne(w => w.Language)
                .HasForeignKey(w => w.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

//ConnectionStrings: {DefaultConnection: "Server=localhost;Database=mydb;Trusted_Connection=True;}