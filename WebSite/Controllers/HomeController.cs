using AspNetCoreHero.ToastNotification.Notyf.Models;
using AutoMapper;
using WebSite.Repositories;
using WebSite.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using NETCore.Encrypt.Extensions;
using System.Diagnostics;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReportRepository _reportRepository;

        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IFileProvider _fileProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ReportRepository reportRepository, IMapper mapper, IConfiguration config, INotyfService notyf, IFileProvider fileProvider, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _config = config;
            _notyf = notyf;
            _fileProvider = fileProvider;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.IsActive == true).ToList();
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> Turkey()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 1 && s.IsActive == true).ToList();  // "Turkey" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public IActionResult Enjoy()
        {
            return View();
        }

        public async Task<IActionResult> Economy()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 5 && s.IsActive == true).ToList();  // "Economy" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> Politics()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 7 && s.IsActive == true).ToList();  // "Politics" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> World()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 3 && s.IsActive == true).ToList();  // "World" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> Sports()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 4 && s.IsActive == true).ToList();  // "Sports" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> Agenda()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 2 && s.IsActive == true).ToList();  // "Agenda" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }

        public async Task<IActionResult> Health()
        {
            var reports = await _reportRepository.GetAllAsync();
            reports = reports.Where(s => s.CategoryId == 6 && s.IsActive == true).ToList();  // "Health" kategoriye göre filtrelendi
            var reportModels = _mapper.Map<List<ReportModel>>(reports);
            return View(reportModels);
        }



        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                _notyf.Error("Girilen Kullanıcı Adı Kayıtlı Değildir!");
                return View(model);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.KeepMe, true);

            if (signInResult.Succeeded)
            {

                await _userManager.AddClaimAsync(user, new Claim("PhotoUrl", user.PhotoUrl));
                return RedirectToAction("Index", "Admin");
            }
            if (signInResult.IsLockedOut)
            {
                _notyf.Error("Kullanıcı Girişi " + user.LockoutEnd + " kadar kısıtlanmıştır!");

                return View(model);
            }
            _notyf.Error("Geçersiz Kullanıcı Adı veya Parola Başarısız Giriş Sayısı :" + await _userManager.GetAccessFailedCountAsync(user) + "/3");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var identityResult = await _userManager.CreateAsync(new() { UserName = model.UserName, Email = model.Email, FullName = model.FullName, PhotoUrl = "default-avatar.png" }, model.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);


                    _notyf.Error(item.Description);
                }

                return View(model);
            }

            // default olarak Uye rolü ekleme
            var user = await _userManager.FindByNameAsync(model.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Uye");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Uye" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Uye");

            _notyf.Success("Üye Kaydı Yapılmıştır. Oturum Açınız");
            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

