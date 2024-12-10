using System.ComponentModel.DataAnnotations;
namespace WebSite.ViewModels
{
    public class UserModel : BaseModel
    {
        [Display(Name = "Adı Soyadı")]
        [Required(ErrorMessage = "Adı Soyadı Giriniz!")]
        public string FullName { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı Giriniz!")]
        public string UserName { get; set; }

        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "E-Posta Giriniz!")]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Giriniz!")]
        public string Password { get; set; }

        [Display(Name = "Yetki")]
        [Required(ErrorMessage = "Yetki Giriniz!")]
        public string RoleName { get; set; }

        [Display(Name = "Fotoğraf")]
        public string PhotoUrl { get; set; }
    }
}
