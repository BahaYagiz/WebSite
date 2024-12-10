﻿using System.ComponentModel.DataAnnotations;

namespace WebSite.ViewModels
{
    public class CategoryModel : BaseModel
    {


        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Kategori Adı Giriniz!")]
        public string Name { get; set; }

    }

}
