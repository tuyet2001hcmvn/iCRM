using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class testcontext : DbContext
    {
        public testcontext()
        {
        }

        public testcontext(DbContextOptions<testcontext> options)
            : base(options)
        {
        }

        public virtual DbSet<RequestEccEmailConfigModel> RequestEccEmailConfigModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.168.100.233;Database=ISD_iCRM_Dev;User Id=sa;Password=123@abcd;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<RequestEccEmailConfigModel>(entity =>
            {
                entity.ToTable("RequestEccEmailConfigModel", "tMasterData");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FromEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FromEmailPassword)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Host)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subject).HasMaxLength(255);

                entity.Property(e => e.ToEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
