using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;
using WebSite.Repositories;
using WebSite.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebSite.Controllers
{
    [Authorize(Roles = "Admin , Yazar")]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ReportRepository _reportRepository;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;

        public CategoryController(CategoryRepository categoryRepository, ReportRepository reportRepository,INotyfService notyf, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _reportRepository = reportRepository;
            
            _notyf = notyf;
            _mapper = mapper;
        }

        // Kategorilerin listelendiği sayfa
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();  
            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);  
            return View(categoryModels);
        }

        // Kategori ekleme sayfası
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Model geçerli değilse, ekleme sayfasını tekrar göster
            }

            var category = _mapper.Map<Category>(model);  // CategoryModel'i Category'ye dönüştür
            await _categoryRepository.AddAsync(category);  // Asenkron olarak kategoriyi ekle
            _notyf.Success("Kategori Eklendi...");  // Başarı mesajı göster
            return RedirectToAction("Index");  // Liste sayfasına yönlendir
        }

        // Kategori güncelleme sayfası
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);  // Asenkron olarak kategori al
            if (category == null)  // Kategori bulunamazsa
            {
                _notyf.Error("Kategori Bulunamadı.");
                return RedirectToAction("Index");
            }

            var categoryModel = _mapper.Map<CategoryModel>(category);  // Category'yi CategoryModel'e dönüştür
            return View(categoryModel);  // View'a gönder
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Model geçerli değilse, güncelleme sayfasını tekrar göster
            }

            var category = await _categoryRepository.GetByIdAsync(model.Id);  // Asenkron olarak kategori al
            if (category == null)  // Kategori bulunamazsa
            {
                _notyf.Error("Kategori Bulunamadı.");
                return RedirectToAction("Index");
            }

            category.Name = model.Name;
            category.IsActive = model.IsActive;
            await _categoryRepository.UpdateAsync(category);  // Asenkron olarak kategoriyi güncelle
            _notyf.Success("Kategori Güncellendi...");  // Başarı mesajı
            return RedirectToAction("Index");  // Liste sayfasına yönlendir
        }

        // Kategori silme sayfası
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);  // Asenkron olarak kategori al
            if (category == null)  // Kategori bulunamazsa
            {
                _notyf.Error("Kategori Bulunamadı.");
                return RedirectToAction("Index");
            }

            var categoryModel = _mapper.Map<CategoryModel>(category);  // Category'yi CategoryModel'e dönüştür
            return View(categoryModel);  // View'a gönder
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryModel model)
        {
            var reports = await _reportRepository.GetAllAsync();  // Ürünleri asenkron olarak al
            if (reports.Count(c => c.CategoryId == model.Id) > 0)  
            {
                _notyf.Error("Üzerinde Ürün Kayıtlı Olan Kategori Silinemez!");  
                return RedirectToAction("Index");  
            }

            await _categoryRepository.DeleteAsync(model.Id);  
            _notyf.Success("Kategori Silindi...");  
            return RedirectToAction("Index");  
        }
    }
}
