using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NETCore.Encrypt.Extensions;
using System.Collections.Specialized;
using WebSite.Models;
using WebSite.Repositories;
using WebSite.ViewModels;

namespace WebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IFileProvider _fileProvider;

        public UserController(UserRepository userRepository, IMapper mapper, IConfiguration config, INotyfService notyf, IFileProvider fileProvider)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
            _notyf = notyf;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            var userModels = _mapper.Map<List<UserModel>>(users);
            return View(userModels);
        }

        public async Task<IActionResult> Add()
        {
            // Role modelini doğrudan Role modelinden çekiyoruz
            var roles = await _userRepository.GetAllAsync(); // GetRolesAsync yerine GetAllAsync kullanıyoruz
            ViewBag.Roles = roles; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserModel model)
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

            var user = new User
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                PhotoUrl = "/admin-tema/assest/img/default-avatar.png",
                Password = MD5Hash(model.Password),
                RoleName = model.RoleName // Role modelini burada kullanıyoruz
            };

            await _userRepository.AddAsync(user);
            _notyf.Success("Üye Eklendi");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);

            // Role modelini doğrudan Role modelinden çekiyoruz
            var roles = await _userRepository.GetAllAsync(); // GetRolesAsync yerine GetAllAsync kullanıyoruz
            ViewBag.Roles = roles;

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.RoleName = model.RoleName; 
            user.Updated = DateTime.Now;

            await _userRepository.UpdateAsync(user);
            _notyf.Success("Üye Güncellendi");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);

            if (user.RoleName == "Admin")
            {
                _notyf.Error("Yönetici Üye Silinemez!");
                return RedirectToAction("Index");
            }
            await _userRepository.DeleteAsync(model.Id);
            _notyf.Success("Üye Silindi");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Profile()
        {
            var userName = User.Claims.First(c => c.Type == "UserName").Value;
            var user = await _userRepository.Where(s => s.UserName == userName).FirstOrDefaultAsync();
            var userModel = _mapper.Map<RegisterModel>(user);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(RegisterModel model)
        {
            var userName = User.Claims.First(c => c.Type == "UserName").Value;
            var user = await _userRepository.Where(s => s.UserName == userName).FirstOrDefaultAsync();

            if (model.Password != model.PasswordConfirm)
            {
                _notyf.Error("Parola Tekrarı Tutarsız!");
                return View(model);
            }

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = MD5Hash(model.Password); 
            }

            user.Updated = DateTime.Now;

            await _userRepository.UpdateAsync(user);
            _notyf.Success("Profil Güncellendi");
            return RedirectToAction("Profile");
        }

        private string MD5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var hashBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            var sb = new System.Text.StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
