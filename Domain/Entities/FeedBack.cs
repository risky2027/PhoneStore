using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FeedBack
    {
        [Required(ErrorMessage = "Укажите Ваше имя")]
        public string Name { get; set; }
        
        [RegularExpression(@"\([0-9]{3}\)\s[0-9]{3}-[0-9]{2}-[0-9]{2}", ErrorMessage = "Неверный формат номера телефона")]
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Неверный формат email")]
        [Required(ErrorMessage = "Укажите адрес электронной почты")]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Напишите свое сообщение")]
        public string Message { get; set; }
    }
}
