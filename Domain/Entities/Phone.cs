using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Phone
    {
        [HiddenInput(DisplayValue = false)] //отображение в редактировании панели админа
        [Display(Name = "ID")]
        public int PhoneId { get; set; }

        [Display(Name = "Марка")]
        [Required(ErrorMessage ="Пожалуйста, введите марку телефона")]
        public string Mark { get; set; }

        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Пожалуйста, введите модель телефона")]
        public string Model { get; set; }

        [DataType(DataType.MultilineText)] //отображение в редактировании панели админа
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание товара")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию товара")]
        public string Category { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение цены товара")]
        public decimal Price { get; set; }
        
        public byte[] ImageData { get; set; }
        
        public string ImageMimeType { get; set; }
    }
}
