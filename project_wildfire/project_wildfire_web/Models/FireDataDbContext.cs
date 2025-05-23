using Microsoft.EntityFrameworkCore;


namespace project_wildfire_web.Models;

public partial class FireDataDbContext : DbContext
{
    public FireDataDbContext(DbContextOptions<FireDataDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AqiStation> AqiStations { get; set; }

    public virtual DbSet<Fire> Fires { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }
    
    public virtual DbSet<UserFireSubscription> UserFireSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AqiStation>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StationId).HasMaxLength(10);
        });

        modelBuilder.Entity<Fire>(entity =>
        {
            entity.HasKey(e => e.FireId).HasName("PK__Fires__E1DECA144C64F9FF");

            entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.RadiativePower).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C60961939");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

        });

        modelBuilder.Entity<UserLocation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLocations_Users");
        });
        modelBuilder.Entity<UserFireSubscription>()
            .HasKey(ufs => new { ufs.UserId, ufs.FireId });

        modelBuilder.Entity<UserFireSubscription>()
            .HasOne(ufs => ufs.User)
            .WithMany(u => u.FireSubscriptions)
            .HasForeignKey(ufs => ufs.UserId);

        modelBuilder.Entity<UserFireSubscription>()
            .HasOne(ufs => ufs.Fire)
            .WithMany(f => f.UserSubscriptions)
            .HasForeignKey(ufs => ufs.FireId);

        modelBuilder.Entity<AqiStation>() //these two lines were added for my nunit test F14
            .HasKey(a => a.StationId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
