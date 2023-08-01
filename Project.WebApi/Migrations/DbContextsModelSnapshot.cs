﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.WebApi.Models;

#nullable disable

namespace Project.WebApi.Migrations
{
    [DbContext(typeof(DbContexts))]
    partial class DbContextsModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Project.WebApi.Models.ImageWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("FileSize")
                        .HasColumnType("int");

                    b.Property<string>("FileType")
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("Varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("ImageWork");
                });
#pragma warning restore 612, 618
        }
    }
}
