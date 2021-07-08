using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.ViewModels
{
    public class NewUserViewModel
    {
        [Required]
        [Display(Name = "Adı")]
        public string FirsName { get; set; }

        [Required]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }


        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "TC Kimlik No")]
        [DataType(DataType.PhoneNumber)]
        public string IdentificationNumber { get; set; }

        [Required]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Bölüm")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "İş Yeri")]
        public string Business { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile ProfileImage { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Onayı")]
        public bool EmailConfirmed { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        public bool Ishome { get; set; }
    }
}
