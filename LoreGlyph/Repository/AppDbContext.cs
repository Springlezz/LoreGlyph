using LoreGlyph.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoreGlyph.Repository
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
            modelBuilder.Entity<UserEntity>()
                .HasKey(e => e.Id);
           
            modelBuilder.Entity<WordEntity>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<LanguageEntity>()
            .HasKey(e => e.Id);

            modelBuilder.Entity<LanguageEntity>()
                .HasMany(l => l.Words)
                .WithOne(w => w.Language)
                .HasForeignKey(w => w.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            ModelTimeConvert<UserEntity>(modelBuilder);
            ModelTimeConvert<WordEntity>(modelBuilder);
            ModelTimeConvert<LanguageEntity>(modelBuilder);            
        }
        private void ModelTimeConvert<T>(ModelBuilder modelBuilder) where T: BaseEntity
        {
            modelBuilder.Entity<T>()
                .Property(x => x.CreatedAt).HasConversion(
                x => x.ToUniversalTime(),
                x => x);
        }
    }
}