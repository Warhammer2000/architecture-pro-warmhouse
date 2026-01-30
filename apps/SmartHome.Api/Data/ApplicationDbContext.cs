using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Models;

namespace SmartHome.Api.Data;

/// <summary>
/// Application database context for Entity Framework Core
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sensor> Sensors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.ToTable("sensors");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Location)
                .HasColumnName("location")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Value)
                .HasColumnName("value")
                .HasDefaultValue(0);

            entity.Property(e => e.Unit)
                .HasColumnName("unit")
                .HasMaxLength(20);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("inactive");

            entity.Property(e => e.LastUpdated)
                .HasColumnName("last_updated")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.Type).HasDatabaseName("idx_sensors_type");
            entity.HasIndex(e => e.Location).HasDatabaseName("idx_sensors_location");
            entity.HasIndex(e => e.Status).HasDatabaseName("idx_sensors_status");
        });
    }
}
