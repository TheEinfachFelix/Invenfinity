using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bin> Bins { get; set; }

    public virtual DbSet<BinSlot> BinSlots { get; set; }

    public virtual DbSet<BinType> BinTypes { get; set; }

    public virtual DbSet<Grid> Grids { get; set; }

    public virtual DbSet<GridPo> GridPos { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=initexample;Username=postgres;Password=initexample");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bin>(entity =>
        {
            entity.HasKey(e => e.BinId).HasName("Bin_pkey");

            entity.ToTable("Bin");

            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.BinTypeId).HasColumnName("BinTypeID");

            entity.HasOne(d => d.BinType).WithMany(p => p.Bins)
                .HasForeignKey(d => d.BinTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Bin_BinTypeID_fkey");
        });

        modelBuilder.Entity<BinSlot>(entity =>
        {
            entity.HasKey(e => new { e.BinId, e.SlotNr }).HasName("BinSlot_pkey");

            entity.ToTable("BinSlot");

            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.PartId).HasColumnName("PartID");

            entity.HasOne(d => d.Bin).WithMany(p => p.BinSlots)
                .HasForeignKey(d => d.BinId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BinSlot_BinID_fkey");

            entity.HasOne(d => d.Part).WithMany(p => p.BinSlots)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BinSlot_PartID_fkey");
        });

        modelBuilder.Entity<BinType>(entity =>
        {
            entity.HasKey(e => e.BinTypeId).HasName("BinType_pkey");

            entity.ToTable("BinType");

            entity.Property(e => e.BinTypeId).HasColumnName("BinTypeID");
        });

        modelBuilder.Entity<Grid>(entity =>
        {
            entity.HasKey(e => e.GridId).HasName("Grid_pkey");

            entity.ToTable("Grid");

            entity.Property(e => e.GridId).HasColumnName("GridID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.Location).WithMany(p => p.Grids)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Grid_LocationID_fkey");
        });

        modelBuilder.Entity<GridPo>(entity =>
        {
            entity.HasKey(e => e.GridPosId).HasName("GridPos_pkey");

            entity.Property(e => e.GridPosId).HasColumnName("GridPosID");
            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.GridId).HasColumnName("GridID");

            entity.HasOne(d => d.Bin).WithMany(p => p.GridPos)
                .HasForeignKey(d => d.BinId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("GridPos_BinID_fkey");

            entity.HasOne(d => d.Grid).WithMany(p => p.GridPos)
                .HasForeignKey(d => d.GridId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("GridPos_GridID_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("Location_pkey");

            entity.ToTable("Location");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.MasterLocationId).HasColumnName("MasterLocationID");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.MasterLocation).WithMany(p => p.InverseMasterLocation)
                .HasForeignKey(d => d.MasterLocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Location_MasterLocationID_fkey");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("Part_pkey");

            entity.ToTable("Part");

            entity.Property(e => e.PartId).HasColumnName("PartID");
            entity.Property(e => e.InventreeId).HasColumnName("InventreeID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
