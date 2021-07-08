using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MezunBilgiSistemi.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MBSUser class
    public class MBSUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Adı")]
        public string FirsName { get; set; }

        [PersonalData]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }


        [PersonalData]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [PersonalData]
        [Display(Name = "TC Kimlik No")]
        [DataType(DataType.PhoneNumber)]
        public string IdentificationNumber { get; set; }

        [PersonalData]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [PersonalData]
        [Display(Name = "Bölüm")]
        public string Department { get; set; }

        [PersonalData]
        [Display(Name = "İş Yeri")]
        public string Business { get; set; }

        [PersonalData]
        [Display(Name = "Meslek")]
        public string Job { get; set; }

        [PersonalData]
        [Display(Name = "Başarı Hikayesi")]
        [DataType(DataType.MultilineText)]
        public string SuccesStory { get; set; }

        [PersonalData]
        [Display(Name = "Profil Fotoğrafı")]
        [DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }
        public bool IsHome { get; set; }
    }
}
