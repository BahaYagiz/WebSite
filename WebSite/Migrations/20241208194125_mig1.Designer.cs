﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebSite.Models;

#nullable disable

namespace WebSite.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241208194125_mig1")]
    partial class mig1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebSite.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            Name = "Türkiye"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = true,
                            Name = "Gündem"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            Name = "Dünya"
                        },
                        new
                        {
                            Id = 4,
                            IsActive = true,
                            Name = "Spor"
                        },
                        new
                        {
                            Id = 5,
                            IsActive = true,
                            Name = "Ekonomi"
                        },
                        new
                        {
                            Id = 6,
                            IsActive = true,
                            Name = "Sağlık"
                        },
                        new
                        {
                            Id = 7,
                            IsActive = true,
                            Name = "Politika"
                        });
                });

            modelBuilder.Entity("WebSite.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Reports");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorName = "Mehmet Yılmaz",
                            CategoryId = 1,
                            Content = "Detaylı içerik...",
                            Description = "Türkiye'deki son gelişmeler ve haberler.",
                            IsActive = true,
                            PhotoUrl = "/uploads/turkiye.jpg",
                            Title = "Türkiye'deki Son Durum"
                        },
                        new
                        {
                            Id = 2,
                            AuthorName = "Ayşe Demir",
                            CategoryId = 2,
                            Content = "Gündem ile ilgili tüm detaylar...",
                            Description = "Ülkedeki gündem maddeleri hakkında detaylar.",
                            IsActive = true,
                            PhotoUrl = "/uploads/gundem.jpg",
                            Title = "Gündemdeki En Önemli Konular"
                        },
                        new
                        {
                            Id = 3,
                            AuthorName = "Ali Veli",
                            CategoryId = 3,
                            Content = "Dünya ekonomisindeki son değişiklikler...",
                            Description = "Dünya ekonomisi üzerindeki etkileyen faktörler.",
                            IsActive = true,
                            PhotoUrl = "/uploads/dunya.jpg",
                            Title = "Dünya Ekonomisinde Son Gelişmeler"
                        },
                        new
                        {
                            Id = 4,
                            AuthorName = "Fatma Kaya",
                            CategoryId = 4,
                            Content = "Sporun geleceği hakkında detaylı bir analiz...",
                            Description = "Yeni sporcuların başarıları.",
                            IsActive = true,
                            PhotoUrl = "/uploads/spor.jpg",
                            Title = "Sporun Yükselen Yıldızları"
                        },
                        new
                        {
                            Id = 5,
                            AuthorName = "Ahmet Öztürk",
                            CategoryId = 5,
                            Content = "Ekonomik kriz ile ilgili çeşitli analizler...",
                            Description = "Ekonomik kriz ve dünya üzerindeki etkileri.",
                            IsActive = true,
                            PhotoUrl = "/uploads/ekonomi.jpg",
                            Title = "Ekonomik Kriz ve Etkileri"
                        });
                });

            modelBuilder.Entity("WebSite.Models.Report", b =>
                {
                    b.HasOne("WebSite.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
