using Microsoft.EntityFrameworkCore;
using SmartAtsCv.Api.Models;

namespace SmartAtsCv.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CvProfile> CvProfiles => Set<CvProfile>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<LanguageItem> Languages => Set<LanguageItem>();
    public DbSet<Certification> Certifications => Set<Certification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<CvProfile>()
            .HasOne(c => c.User)
            .WithMany(u => u.CvProfiles)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Experience>()
            .HasOne(e => e.CvProfile)
            .WithMany(c => c.Experiences)
            .HasForeignKey(e => e.CvProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Education>()
            .HasOne(e => e.CvProfile)
            .WithMany(c => c.Educations)
            .HasForeignKey(e => e.CvProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Skill>()
            .HasOne(s => s.CvProfile)
            .WithMany(c => c.Skills)
            .HasForeignKey(s => s.CvProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LanguageItem>()
            .HasOne(l => l.CvProfile)
            .WithMany(c => c.Languages)
            .HasForeignKey(l => l.CvProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Certification>()
            .HasOne(cert => cert.CvProfile)
            .WithMany(c => c.Certifications)
            .HasForeignKey(cert => cert.CvProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
