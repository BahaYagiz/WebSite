using WebSite.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        // Constructor, base sınıfa AppDbContext'i geçiriyoruz
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        // Role verilerini asenkron olarak almayı sağlayan metod
        public async Task<List<Role>> GetRolesAsync()
        {
            // Roles tablosundaki tüm verileri listeleyip geri döndürüyoruz
            return await _context.Roles.ToListAsync();
        }

        // Eğer rol eklemek isterseniz (isteğe bağlı)
        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        // Eğer rol silmek isterseniz (isteğe bağlı)
        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        // Eğer rol güncellemek isterseniz (isteğe bağlı)
        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
