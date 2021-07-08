using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Bölüm { get; set; }

        [Required]
        public string Meslek { get; set; }
        public string İşyeri { get; set; }

        [Required]
        public string Cinsiyet { get; set; }

        [Required]
        public string Telefon { get; set; }
        public bool EmailConfirmed { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool Ishome { get; set; }


        public string Adres { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
