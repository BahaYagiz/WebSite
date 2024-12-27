using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;
using WebSite.Repositories;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSite.ViewModels;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebSite.Hubs;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebSite.Controllers
{
    [Authorize(Roles = "Admin , Yazar")]
    public class ReportController : Controller
    {
        private readonly ReportRepository _reportRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IHubContext<GeneralHub> _generalHub;

        public ReportController(ReportRepository reportRepository, CategoryRepository categoryRepository, IMapper mapper, INotyfService notyf, IHubContext<GeneralHub> generalHub)
        {
            _reportRepository = reportRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _generalHub = generalHub;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _reportRepository.GetAllAsync();
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
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
            try
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
                }

                var report = _mapper.Map<Report>(model);
                await _reportRepository.AddAsync(report);

                int repCount = _reportRepository.Where(c => c.IsActive == true).Count();
                await _generalHub.Clients.All.SendAsync("onReportAdd", repCount);

                _notyf.Success("Haber başarıyla eklendi!");
                return RedirectToAction("Index");
            }
            catch
            {
                _notyf.Error("Haber eklenirken bir hata oluştu.");
                return View(model);
            }
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
            var reportModel = _mapper.Map<ReportModel>(report);
            return View(reportModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReportModel model)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var photoFile = Request.Form.Files[0];

                    if (photoFile != null && photoFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(model.PhotoUrl))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.PhotoUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        var fileName = Path.GetFileName(photoFile.FileName);
                        var uniqueFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }

                        model.PhotoUrl = "/uploads/" + uniqueFileName;
                    }
                }

                var report = _mapper.Map<Report>(model);
                await _reportRepository.UpdateAsync(report);

                int repCount = _reportRepository.Where(c => c.IsActive == true).Count();
                await _generalHub.Clients.All.SendAsync("onReportUpdate", repCount);

                _notyf.Success("Haber başarıyla güncellendi!");
                return RedirectToAction("Index");
            }
            catch
            {
                _notyf.Error("Haber güncellenirken bir hata oluştu.");
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            var reportModel = _mapper.Map<ReportModel>(report);
            return View(reportModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReportModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.PhotoUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.PhotoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                await _reportRepository.DeleteAsync(model.Id);

                int repCount = _reportRepository.Where(c => c.IsActive == true).Count();
                await _generalHub.Clients.All.SendAsync("onReportDelete", repCount);

                _notyf.Success("Haber başarıyla silindi!");
                return RedirectToAction("Index");
            }
            catch
            {
                _notyf.Error("Haber silinirken bir hata oluştu.");
                return View(model);
            }
        }
    }
}
