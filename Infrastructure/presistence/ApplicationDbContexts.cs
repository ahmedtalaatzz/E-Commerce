using Domain.Entities;
using e_commerce.core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.infrastructure.DbContxt
{
    /// <summary>
    /// Database context for the application
    /// </summary>
    public class ApplicationDbContexts : DbContext
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options) : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Brand entity
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brands");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameAr).IsRequired().HasMaxLength(200);
                entity.Property(e => e.NameEn).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.HasIndex(e => new { e.IsActive, e.IsDeleted });
                entity.HasIndex(e => e.NameEn);
                entity.HasIndex(e => e.CreatedAt);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configure Banner entity
            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("banners");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.HasIndex(e => new { e.IsActive, e.IsDeleted });
                entity.HasIndex(e => e.CreatedAt);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configure Partner entity
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partners");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameAr).IsRequired().HasMaxLength(200);
                entity.Property(e => e.NameEn).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.HasIndex(e => new { e.IsActive, e.IsDeleted });
                entity.HasIndex(e => e.NameEn);
                entity.HasIndex(e => e.CreatedAt);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}
