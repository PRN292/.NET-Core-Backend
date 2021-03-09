using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class StrangerDetectionContext : DbContext
    {
        public StrangerDetectionContext()
        {
        }

        public StrangerDetectionContext(DbContextOptions<StrangerDetectionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAccount> TblAccounts { get; set; }
        public virtual DbSet<TblEncoding> TblEncodings { get; set; }
        public virtual DbSet<TblKnownPerson> TblKnownPeople { get; set; }
        public virtual DbSet<TblNotificaton> TblNotificatons { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NATTON\\NATTON;Initial Catalog=StrangerDetection;User Id=sa;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__TblAccou__F3DBC573D741E41B");

                entity.ToTable("TblAccount");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("address");

                entity.Property(e => e.IdentificationCardBackImageName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identificationCardBackImageName");

                entity.Property(e => e.IdentificationCardFrontImageName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identificationCardFrontImageName");

                entity.Property(e => e.IsLogin).HasColumnName("isLogin");

                entity.Property(e => e.IsRememberMe).HasColumnName("isRememberMe");

                entity.Property(e => e.KnownPersonId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("knownPersonID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.ProfileImageName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("profileImageName");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.HasOne(d => d.KnownPerson)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.KnownPersonId)
                    .HasConstraintName("FK__TblAccoun__known__3F466844");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__TblAccoun__roleI__3E52440B");
            });

            modelBuilder.Entity<TblEncoding>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Encodings)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("encodings");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("image_name")
                    .IsFixedLength(true);

                entity.Property(e => e.KnownPersonId)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("knownPersonID");

                entity.HasOne(d => d.KnownPerson)
                    .WithMany(p => p.TblEncodings)
                    .HasForeignKey(d => d.KnownPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblEncodi__known__3B75D760");
            });

            modelBuilder.Entity<TblKnownPerson>(entity =>
            {
                entity.ToTable("TblKnownPerson");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TblNotificaton>(entity =>
            {
                entity.ToTable("TblNotificaton");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnName("createTime");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("image_name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("TblRole");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
