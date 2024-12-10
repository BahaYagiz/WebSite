using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;
using WebSite.Repositories;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSite.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace WebSite.Controllers
{
    [Authorize(Roles = "Admin , Yazar")]
    public class ReportController : Controller
    {
        private readonly ReportRepository _reportRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ReportController(ReportRepository reportRepository, CategoryRepository categoryRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _reportRepository.GetAllAsync();
            var reportModels = _mapper.Map<List<ReportModel>>(reports); // Report -> ReportModel dönüşümü
            return View(reportModels);
        }

        public async Task<IActionResult> Add()
        {
            var categories = (await _categoryRepository.GetAllAsync())
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReportModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var photoFile = Request.Form.Files[0];
                    if (photoFile != null && photoFile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", photoFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }
                        model.PhotoUrl = "/uploads/" + photoFile.FileName;
                    }
                    else
                    {
                        ModelState.AddModelError("PhotoUrl", "Fotoğraf yüklemek zorunludur.");
                    }
                }

                // ReportModel'dan Report'a dönüştürme
                var report = _mapper.Map<Report>(model);
                await _reportRepository.AddAsync(report);

                TempData["Success"] = "Haber başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            var categories = (await _categoryRepository.GetAllAsync())
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            ViewBag.Categories = categories;
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var categories = (await _categoryRepository.GetAllAsync())
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            ViewBag.Categories = categories;
            var report = await _reportRepository.GetByIdAsync(id);
            var reportModel = _mapper.Map<ReportModel>(report); // Report -> ReportModel dönüşümü
            return View(reportModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReportModel model)
        {
            if (ModelState.IsValid)
            {
                // Fotoğraf güncelleme işlemi
                if (Request.Form.Files.Count > 0)
                {
                    var photoFile = Request.Form.Files[0];

                    if (photoFile != null && photoFile.Length > 0)
                    {
                        // Eski fotoğrafı sil
                        if (!string.IsNullOrEmpty(model.PhotoUrl))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.PhotoUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Yeni fotoğrafı kaydet
                        var fileName = Path.GetFileName(photoFile.FileName);
                        var uniqueFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }

                        model.PhotoUrl = "/uploads/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("PhotoUrl", "Fotoğraf yüklemek zorunludur.");
                    }
                }

                if (string.IsNullOrEmpty(model.PhotoUrl))
                {
                    model.PhotoUrl = null;
                }

                // ReportModel'dan Report'a dönüşüm ve güncelleme
                var report = _mapper.Map<Report>(model);
                await _reportRepository.UpdateAsync(report);

                TempData["Success"] = "Haber başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            var categories = (await _categoryRepository.GetAllAsync())
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            ViewBag.Categories = categories;
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            var reportModel = _mapper.Map<ReportModel>(report); // Report -> ReportModel dönüşümü
            return View(reportModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReportModel model)
        {
            // Fotoğrafı sil
            if (!string.IsNullOrEmpty(model.PhotoUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.PhotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Veritabanından silme
            await _reportRepository.DeleteAsync(model.Id);

            // Başarı mesajı
            TempData["Success"] = "Haber başarıyla silindi!";
            return RedirectToAction("Index");
        }
    }
}
