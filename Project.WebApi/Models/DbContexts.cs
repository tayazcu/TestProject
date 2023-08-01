using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Project.WebApi.Models
{
    public class DbContexts : DbContext
    {
        public DbContexts(DbContextOptions<DbContexts> options): base(options)
        {
        }

        public DbSet<ImageWork> ImageWork { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageWork>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FileName)
                    .HasMaxLength(2000)
                    .IsUnicode(true)
                    .IsRequired(true)
                    .HasColumnType("nvarchar(2000)");

                entity.Property(c => c.Image)
                .HasColumnType("Varbinary(max)");

                entity.Property(c => c.FileType)
                    .HasColumnType("varchar(50)");
            });

        }
    }
}
