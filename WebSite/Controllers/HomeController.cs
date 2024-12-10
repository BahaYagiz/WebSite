using AutoMapper;
using WebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSite.Models;
using WebSite.Repositories;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReportRepository _reportRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IFileProvider _fileProvider;
        private readonly CategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, ReportRepository reportRepository, IMapper mapper, UserRepository userRepository, IConfiguration config, INotyfService notyf, IFileProvider fileProvider, CategoryRepository categoryRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _config = config;
            _notyf = notyf;
            _fileProvider = fileProvider;
            _categoryRepository = categoryRepository;
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
        public IActionResult Login(LoginModel model)
        {
            // MD5 ile şifreyi hash'le
            var hashedpass = MD5Hash(model.Password);

            // Kullanıcıyı veritabanında bul
            var user = _userRepository.Where(s => s.UserName == model.UserName && s.Password == hashedpass).SingleOrDefault();

            if (user == null)
            {
                // Kullanıcı adı veya parola yanlışsa hata mesajı göster
                _notyf.Error("Kullanıcı Adı veya Parola Geçersizdir!");
                return View();
            }

            // Kullanıcı başarıyla giriş yaparsa başarılı bildirim göster
            _notyf.Success("Başarıyla giriş yaptınız!");

            // Kullanıcı bilgilerini Claim olarak ayarla
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim("UserName", user.UserName),
                new Claim("PhotoUrl", user.PhotoUrl),
                new Claim("Email", user.Email),
            };

            // Kimlik bilgileri oluştur
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Giriş özelliklerini ayarla
            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = model.KeepMe
            };

            // Oturum aç
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            // Admin sayfasına yönlendir
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (_userRepository.Where(s => s.UserName == model.UserName).Count() > 0)
            {
                _notyf.Error("Girilen Kullanıcı Adı Kayıtlıdır!");
                return View(model);
            }
            if (_userRepository.Where(s => s.Email == model.Email).Count() > 0)
            {
                _notyf.Error("Girilen E-Posta Adresi Kayıtlıdır!");
                return View(model);
            }

            var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var photoUrl = "admin-tema/assest/img/default-avatar.png";
            if (model.PhotoFile != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.PhotoFile.FileName);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "userPhotos").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                model.PhotoFile.CopyTo(stream);
                photoUrl = filename;
            }

            var hashedpass = MD5Hash(model.Password);
            var user = new User();
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Password = hashedpass;
            user.Email = model.Email;
            user.PhotoUrl = photoUrl;
            user.RoleName = "Uye";
            await _userRepository.AddAsync(user);

            _notyf.Success("Üye Kaydı Yapılmıştır. Oturum Açınız");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
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

        // Şifreyi MD5 ile hash'le
        public string MD5Hash(string pass)
        {
            var salt = _config.GetValue<string>("AppSettings:MD5Salt");
            var password = pass + salt;
            var hashed = password.MD5();
            return hashed;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

