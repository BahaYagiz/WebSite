using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;
using WebSite.Repositories;
using WebSite.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebSite.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleRepository _roleRepository;
        private readonly INotyfService _notyf;

        public RoleController(RoleRepository roleRepository, INotyfService notyf)
        {
            _roleRepository = roleRepository;
            _notyf = notyf;
        }

        // Rolleri listele
        public async Task<IActionResult> Index()
        {
            var roles = await _roleRepository.GetAllAsync();
            var roleViewModels = roles.Select(r => new RoleModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(roleViewModels);
        }

        // Yeni rol ekle
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni rol oluştur
                var role = new Role
                {
                    Name = model.Name
                };

                await _roleRepository.AddAsync(role);
                _notyf.Success("Yeni rol başarıyla eklendi!");
                return RedirectToAction("Index");  // Başarıyla ekledikten sonra rol listesini göster
            }
            return View(model);
        }

        // Rolü düzenle
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();  // Eğer rol bulunamazsa hata döndür
            }

            var model = new RoleModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleRepository.GetByIdAsync(model.Id);
                if (role == null)
                {
                    return NotFound();  // Eğer rol bulunamazsa hata döndür
                }

                role.Name = model.Name;  // Rol ismini güncelle

                await _roleRepository.UpdateAsync(role);
                _notyf.Success("Rol başarıyla güncellendi!");
                return RedirectToAction("Index");  // Başarıyla güncellenmiş rol listesini göster
            }
            return View(model);
        }

        // Rolü sil
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                _notyf.Error("Silinmek istenen rol bulunamadı!");
                return RedirectToAction("Index");
            }

            await _roleRepository.DeleteAsync(id);  // Burada role.Id yerine id'yi kullanıyoruz
            _notyf.Success("Rol başarıyla silindi!");
            return RedirectToAction("Index");
        }
    }
}
