using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FloorAuth
{
    public partial class floorfixturedbContext : DbContext
    {
        public floorfixturedbContext()
        {
        }

        public floorfixturedbContext(DbContextOptions<floorfixturedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user id=root;password=root;database=floorfixturedb;charset=utf8;pooling=false", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Fixture>(entity =>
            {
                entity.ToTable("fixture");

                entity.HasIndex(e => e.FloorId, "FloorId_idx");

                entity.Property(e => e.FixtureId).ValueGeneratedNever();

                entity.Property(e => e.ClassName).HasMaxLength(255);

                entity.Property(e => e.MacAddress).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Xaxis).HasColumnName("XAxis");

                entity.Property(e => e.Yaxis).HasColumnName("YAxis");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.Fixtures)
                    .HasForeignKey(d => d.FloorId)
                    .HasConstraintName("FloorId");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.ToTable("floor");

                entity.Property(e => e.FloorId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.FloorPlanUrl).HasMaxLength(255);

                entity.Property(e => e.ParentFloorId).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
