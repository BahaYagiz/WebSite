using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using WebSite.Models;

namespace WebSite.Repositories
{
    public class ReportRepository : GenericRepository<Report>
    {
        public ReportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task UpdateReportWithPhotoAsync(Report report, string  newPhotoFilePath)
        {
            // Eski fotoğrafı sil
            if (!string.IsNullOrEmpty(report.PhotoUrl))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.PhotoUrl.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            // Yeni fotoğraf yolunu ayarla
            if (!string.IsNullOrEmpty(newPhotoFilePath))
            {
                report.PhotoUrl = newPhotoFilePath;
            }

            // Report'u güncelle
            _dbSet.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}
