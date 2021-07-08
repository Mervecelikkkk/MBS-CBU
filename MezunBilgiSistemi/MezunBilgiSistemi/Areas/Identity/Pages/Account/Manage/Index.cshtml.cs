using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MezunBilgiSistemi.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MezunBilgiSistemi.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<MBSUser> _userManager;
        private readonly SignInManager<MBSUser> _signInManager;
        public IndexModel(
            UserManager<MBSUser> userManager,
            SignInManager<MBSUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefon Numarası")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Adı")]
            public string FirsName { get; set; }

            [Required]
            [Display(Name = "Soyadı")]
            public string LastName { get; set; }

            [Display(Name = "TC Kimlik No")]
            [DataType(DataType.PhoneNumber)]
            public string IdentificationNumber { get; set; }

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

        }

        private async Task LoadAsync(MBSUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Input = new InputModel
            {
                FirsName=user.FirsName,
                LastName=user.LastName,
                IdentificationNumber=user.IdentificationNumber,
                Gender=user.Gender,
                Job=user.Job,
                Department=user.Department,
                Business=user.Business,
                SuccesStory=user.SuccesStory,
                PhoneNumber = phoneNumber
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.FirsName != user.FirsName)
            {
                user.FirsName = Input.FirsName;  
            }
            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }
            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
            }
            if (Input.Department != user.Department)
            {
                user.Department = Input.Department;
            }
            if (Input.Business != user.Business)
            {
                user.Business = Input.Business;
            }
            if (Input.SuccesStory != user.SuccesStory)
            {
                user.SuccesStory = Input.SuccesStory;
            }
            if (Input.Job != user.Job)
            {
                user.Job = Input.Job;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profiliniz Başarı İle Güncellendi";
            return RedirectToPage();
        }       
    }
}
