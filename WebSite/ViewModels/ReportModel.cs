using System.ComponentModel.DataAnnotations;

namespace WebSite.ViewModels
{
    public class ReportModel:BaseModel
    {
        

        [Display(Name = "Haber Başlığı")]
        [Required(ErrorMessage = "Haber Başlığı Giriniz!")]
        public string Title { get; set; }

        [Display(Name = "Haber Açıklaması")]
        [Required(ErrorMessage = "Haber Açıklaması Giriniz!")]
        public string Description { get; set; }

        [Display(Name = "Haber İçeriği")]
        [Required(ErrorMessage = "Haber İçeriği Giriniz!")]
        public string Content { get; set; }

        [Display(Name = "Yazar Adı")]
        [Required(ErrorMessage = "Yazar Adı Giriniz!")]
        public string AuthorName { get; set; }

        [Display(Name = "Haber Fotoğrafı")]
        [Required(ErrorMessage = "Haber Fotoğrafı Giriniz!")]
        public IFormFile PhotoFile { get; set; }


        [Display(Name = "Kategori Numarası")]
        [Required(ErrorMessage = "Kategori Numarası Giriniz!")]
        public int CategoryId { get; set; }

        public string PhotoUrl { get; set; }
    }
}
