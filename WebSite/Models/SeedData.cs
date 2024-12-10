using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace WebSite.Models
{

    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Türkiye", IsActive = true },
                new Category() { Id = 2, Name = "Gündem", IsActive = true },
                new Category() { Id = 3, Name = "Dünya", IsActive = true },
                new Category() { Id = 4, Name = "Spor", IsActive = true },
                new Category() { Id = 5, Name = "Ekonomi", IsActive = true },
                new Category() { Id = 6, Name = "Sağlık", IsActive = true },
                new Category() { Id = 7, Name = "Politika", IsActive = true }
            );


            modelBuilder.Entity<Report>().HasData(
     new Report()
     {
         Id = 1,
         Title = "Türkiye'deki Son Durum",
         Description = "Türkiye'deki son gelişmeler ve haberler.",
         Content = "Detaylı içerik...",
         AuthorName = "Mehmet Yılmaz",
         PhotoUrl = "/uploads/turkiye.jpg",  // Güncel yol
         IsActive = true,
         CategoryId = 1
     },
     new Report()
     {
         Id = 2,
         Title = "Gündemdeki En Önemli Konular",
         Description = "Ülkedeki gündem maddeleri hakkında detaylar.",
         Content = "Gündem ile ilgili tüm detaylar...",
         AuthorName = "Ayşe Demir",
         PhotoUrl = "/uploads/gundem.jpg",  // Güncel yol
         IsActive = true,
         CategoryId = 2
     },
     new Report()
     {
         Id = 3,
         Title = "Dünya Ekonomisinde Son Gelişmeler",
         Description = "Dünya ekonomisi üzerindeki etkileyen faktörler.",
         Content = "Dünya ekonomisindeki son değişiklikler...",
         AuthorName = "Ali Veli",
         PhotoUrl = "/uploads/dunya.jpg",  // Güncel yol
         IsActive = true,
         CategoryId = 3
     },
     new Report()
     {
         Id = 4,
         Title = "Sporun Yükselen Yıldızları",
         Description = "Yeni sporcuların başarıları.",
         Content = "Sporun geleceği hakkında detaylı bir analiz...",
         AuthorName = "Fatma Kaya",
         PhotoUrl = "/uploads/spor.jpg",  // Güncel yol
         IsActive = true,
         CategoryId = 4
     },
     new Report()
     {
         Id = 5,
         Title = "Ekonomik Kriz ve Etkileri",
         Description = "Ekonomik kriz ve dünya üzerindeki etkileri.",
         Content = "Ekonomik kriz ile ilgili çeşitli analizler...",
         AuthorName = "Ahmet Öztürk",
         PhotoUrl = "/uploads/ekonomi.jpg",  // Güncel yol
         IsActive = true,
         CategoryId = 5
     }
 );

        }


    }
}

