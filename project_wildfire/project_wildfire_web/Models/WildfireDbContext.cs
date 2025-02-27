using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;

namespace project_wildfire_web.Models;

public partial class WildfireDbContext : DbContext
{
    public WildfireDbContext(DbContextOptions<WildfireDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FireDatum> FireData { get; set; }

    public virtual DbSet<SavedLocation> SavedLocations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeatherDatum> WeatherData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FireDatum>(entity =>
        {
            entity.HasKey(e => e.FireId).HasName("PK__FireData__E1DECA144B6D0E6B");

            entity.Property(e => e.FireId).ValueGeneratedNever();
            entity.Property(e => e.RadiativePower).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Weather).WithMany(p => p.FireData)
                .HasForeignKey(d => d.WeatherId)
                .HasConstraintName("FK_FireData_WeatherData");
        });

        modelBuilder.Entity<SavedLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__SavedLoc__E7FEA497C6215128");

            entity.ToTable("SavedLocation");

            entity.Property(e => e.LocationId).ValueGeneratedNever();
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C8002BA03");

            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            
            entity.HasMany(d => d.Fires).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFireSubscription",
                    r => r.HasOne<FireDatum>().WithMany()
                        .HasForeignKey("FireId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserFireSubscription_FireData"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserFireSubscription_User"),
                    j =>
                    {
                        j.HasKey("UserId", "FireId").HasName("PK__UserFire__499520EDB6A35324");
                        j.ToTable("UserFireSubscription");
                    });

            entity.HasMany(d => d.Locations).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserLocation",
                    r => r.HasOne<SavedLocation>().WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserLocation_SavedLocation"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserLocation_User"),
                    j =>
                    {
                        j.HasKey("UserId", "LocationId").HasName("PK__UserLoca__79F72605BDDE839A");
                        j.ToTable("UserLocation");
                    });
        });

        modelBuilder.Entity<WeatherDatum>(entity =>
        {
            entity.HasKey(e => e.WeatherId).HasName("PK__WeatherD__0BF97BF566B917FE");

            entity.Property(e => e.WeatherId).ValueGeneratedNever();
            entity.Property(e => e.WindSpeedDirection)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
