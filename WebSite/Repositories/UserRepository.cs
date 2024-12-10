using WebSite.Models;
using Microsoft.EntityFrameworkCore;

namespace WebSite.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        // Kullanıcıyı ID'si ile silen metot
        public void Delete(int userId)
        {
            var user = _context.Users.Find(userId);  // ID'ye göre kullanıcıyı bul
            if (user != null)
            {
                _context.Users.Remove(user);  // Kullanıcıyı sil
                _context.SaveChanges();  // Değişiklikleri veritabanına kaydet
            }
        }
    }
}
