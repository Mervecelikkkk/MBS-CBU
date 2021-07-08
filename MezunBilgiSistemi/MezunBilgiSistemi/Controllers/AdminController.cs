using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MezunBilgiSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MezunBilgiSistemi.Areas.Identity.Data;
using MezunBilgiSistemi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;

namespace MezunBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Context c = new Context();
        private RoleManager<IdentityRole> roleManager;
        private UserManager<MBSUser> userManager;
        private IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminController(RoleManager<IdentityRole> _roleManager, UserManager<MBSUser> _userManager, IEmailSender _emailSender, IWebHostEnvironment hostEnvironment)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            emailSender = _emailSender;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Kullanıcılar(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var users = from s in userManager.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s =>
                                    s.LastName.Contains(searchString) ||
                                    s.FirsName.Contains(searchString) ||
                                    s.Address.Contains(searchString) ||
                                    s.Business.Contains(searchString) |
                                    s.IdentificationNumber.Contains(searchString) ||
                                    s.Department.Contains(searchString) ||
                                    s.PhoneNumber.Contains(searchString)
                                    );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.FirsName);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.DateOfBirth);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.DateOfBirth);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 8;
            return View(await PaginatedList<MBSUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string uniqueFileName = UploadedUserFile(model);
                var user = new MBSUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Address = model.Address,
                    FirsName = model.FirsName,
                    LastName = model.LastName,
                    Department = model.Department,
                    Business = model.Business,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = model.EmailConfirmed,
                    IdentificationNumber = model.IdentificationNumber,
                    ProfilePicture = uniqueFileName,
                    IsHome=model.Ishome
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Kullanıcılar");
                }
                c.Add(user);
                await c.SaveChangesAsync();
                return RedirectToAction("Kullanıcılar");
            }
            return View("Kullanıcılar");
            
            //var result = await userManager.CreateAsync(user,model.Password);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Kullanıcılar");
            //}

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError("", error.Description);
            //}
            //return View(model);
        }
        private string UploadedUserFile(NewUserViewModel u)
        {
            string uniqueFileName = null;

            if (u.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/user");
                uniqueFileName = "_" + u.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    u.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        [HttpGet]
        public string GeneratePassword()
        {
            var options = userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Adres = user.Address,
                FirstName = user.FirsName,
                LastName = user.LastName,
                Bölüm = user.Department,
                İşyeri = user.Business,
                Cinsiyet = user.Gender,
                Telefon = user.PhoneNumber,
                Meslek = user.Job,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles,
                EmailConfirmed = user.EmailConfirmed,
                Ishome=user.IsHome

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Address = model.Adres;
                user.FirsName = model.FirstName;
                user.LastName = model.LastName;
                user.Department = model.Bölüm;
                user.Business = model.İşyeri;
                user.Job = model.Meslek;
                user.PhoneNumber = model.Telefon;
                user.Gender = model.Cinsiyet;
                user.EmailConfirmed = model.EmailConfirmed;
                user.IsHome = model.Ishome;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Kullanıcılar");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRoleViewModel
                {
                    UserId = role.Id,
                    UserName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult>ManageUserRoles(List<UserRoleViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
            model.Where(x => x.IsSelected).Select(y => y.UserName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Kullanıcılar");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Kullanıcılar");
            }
        }
        public async Task<ActionResult> Etkinlikler(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var etkinlikler = from s in c.Etkinliklers
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                etkinlikler = etkinlikler.Where(s =>
                                    s.EtkinlikAd.Contains(searchString) ||
                                    s.EtkinlikAciklama.Contains(searchString) ||
                                    s.Tarih.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    etkinlikler = etkinlikler.OrderByDescending(s => s.EtkinlikAd);
                    break;
                case "Date":
                    etkinlikler = etkinlikler.OrderBy(s => s.Tarih);
                    break;
                case "date_desc":
                    etkinlikler = etkinlikler.OrderByDescending(s => s.Tarih);
                    break;
                default:
                    etkinlikler = etkinlikler.OrderBy(s => s.EtkinlikAciklama);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Etkinlikler>.CreateAsync(etkinlikler.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpGet]
        public IActionResult EtkinlikEkle()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EtkinlikEkle(EtkinlikViewModel e)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(e);
                var etkinlik = new Etkinlikler
                {
                    EtkinlikAd = e.EtkinlikAd,
                    EtkinlikAciklama = e.EtkinlikAciklama,
                    EtkinlikFotoLink = uniqueFileName,
                    IsHome=e.Ishome,
                    Tarih = DateTime.Today
                };
                c.Etkinliklers.Add(etkinlik);
                await c.SaveChangesAsync();
                return RedirectToAction("Etkinlikler");
            }
            return View();         
        }

        private string UploadedFile(EtkinlikViewModel e)
        {
            string uniqueFileName = null;

            if (e.EtkinlikFoto != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/etkinlikler");
                uniqueFileName = "_" + e.EtkinlikFoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    e.EtkinlikFoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string UploadedHaberFile(HaberViewModel h)
        {
            string uniqueFileName = null;

            if (h.HaberImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/haberler");
                uniqueFileName = "_" + h.HaberImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    h.HaberImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string UploadedDuyuruFile(DuyuruViewModel d)
        {
            string uniqueFileName = null;

            if (d.DuyuruImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/duyurular");
                uniqueFileName = "_" + d.DuyuruImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    d.DuyuruImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult DeleteEtkinlik(int id)
        {
            var etkinlik = c.Etkinliklers.Find(id);
            c.Etkinliklers.Remove(etkinlik);
            c.SaveChanges();
            return RedirectToAction("Etkinlikler");
        }
        public IActionResult EditEtkinlik(int id)
        {
            var etkinlik = c.Etkinliklers.Find(id);
            return View(etkinlik);
        }
        [HttpPost]
        public async Task<IActionResult> EditEtkinlik(Etkinlikler e, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var etkinlik = c.Etkinliklers.Find(e.EtkinlikId);
                etkinlik.EtkinlikAd = e.EtkinlikAd;
                etkinlik.EtkinlikAciklama = e.EtkinlikAciklama;
                etkinlik.IsHome = e.IsHome;
                //etkinlik.EtkinlikFotoLink = e.EtkinlikFotoLink;
                //if (file != null)
                //{
                //    var extention = Path.GetExtension(file.FileName);
                //    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                //    etkinlik.EtkinlikFotoLink = randomName;
                //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Etkinlikler", randomName);

                //    using (var stream = new FileStream(path, FileMode.Create))
                //    {
                //        await file.CopyToAsync(stream);
                //    }
                //}
                c.SaveChanges();
                return RedirectToAction("Etkinlikler");
            }
            return View(e);
        }
        public async Task<ActionResult> Haberler(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var haberler = from s in c.Haberlers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                haberler = haberler.Where(s =>
                                    s.HaberAd.Contains(searchString) ||
                                    s.HaberAciklama.Contains(searchString) ||
                                    s.Tarih.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    haberler = haberler.OrderByDescending(s => s.HaberAd);
                    break;
                case "Date":
                    haberler = haberler.OrderBy(s => s.Tarih);
                    break;
                case "date_desc":
                    haberler = haberler.OrderByDescending(s => s.Tarih);
                    break;
                default:
                    haberler = haberler.OrderBy(s => s.HaberAciklama);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Haberler>.CreateAsync(haberler.AsNoTracking(), pageNumber ?? 1, pageSize));
            //var haberler = c.Haberlers.ToList();
            //return View(haberler);
        }
        [HttpGet]
        public IActionResult HaberEkle()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HaberEkle(HaberViewModel haber)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedHaberFile(haber);
                var haberler = new Haberler
                {
                    HaberAd=haber.HaberAd,
                    HaberAciklama=haber.HaberAciklama,
                    HaberFotoLink=uniqueFileName,
                    IsHome=haber.Ishome,
                    Tarih = DateTime.Today
                };
                c.Haberlers.Add(haberler);
                await c.SaveChangesAsync();
                return RedirectToAction("Haberler");
            }
            return View();
        }
        public IActionResult DeleteHaber(int id)
        {
            var haber = c.Haberlers.Find(id);
            c.Haberlers.Remove(haber);
            c.SaveChanges();
            return RedirectToAction("Haberler");
        }
        public IActionResult EditHaber(int id)
        {
            var haber = c.Haberlers.Find(id);
            return View(haber);
        }
        [HttpPost]
        public async Task<IActionResult> EditHaber(Haberler h, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var haber = c.Haberlers.Find(h.HaberID);
                haber.HaberAd = h.HaberAd;
                haber.HaberAciklama = h.HaberAciklama;
                haber.IsHome = h.IsHome;
                //etkinlik.EtkinlikFotoLink = e.EtkinlikFotoLink;
                //if (file != null)
                //{
                //    var extention = Path.GetExtension(file.FileName);
                //    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                //    etkinlik.EtkinlikFotoLink = randomName;
                //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Etkinlikler", randomName);

                //    using (var stream = new FileStream(path, FileMode.Create))
                //    {
                //        await file.CopyToAsync(stream);
                //    }
                //}
                c.SaveChanges();
                return RedirectToAction("Haberler");
            }
            return View(h);
        }

        public async Task<ActionResult> Duyurular(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var duyurular = from s in c.Duyurulars
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                duyurular = duyurular.Where(s =>
                                    s.DuyuruAd.Contains(searchString) ||
                                    s.DuyuruAciklama.Contains(searchString) ||
                                    s.Tarih.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    duyurular = duyurular.OrderByDescending(s => s.DuyuruAd);
                    break;
                case "Date":
                    duyurular = duyurular.OrderBy(s => s.Tarih);
                    break;
                case "date_desc":
                    duyurular = duyurular.OrderByDescending(s => s.Tarih);
                    break;
                default:
                    duyurular = duyurular.OrderBy(s => s.DuyuruAciklama);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Duyurular>.CreateAsync(duyurular.AsNoTracking(), pageNumber ?? 1, pageSize));
            //var duyurular = c.Duyurulars.ToList();
            //return View(duyurular);
        }

        [HttpGet]
        public IActionResult DuyuruEkle()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DuyuruEkle(DuyuruViewModel duyuru)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedDuyuruFile(duyuru);
                var duyurular = new Duyurular
                {
                    DuyuruAd = duyuru.DuyuruAd,
                    DuyuruAciklama=duyuru.DuyuruAciklama,
                    DuyuruFotoLink=uniqueFileName,
                    IsHome=duyuru.Ishome,
                    Tarih = DateTime.Today
                };
                c.Duyurulars.Add(duyurular);
                await c.SaveChangesAsync();
                return RedirectToAction("Duyurular");
            }
            return View();
        }
        public IActionResult DeleteDuyuru(int id)
        {
            var duyuru = c.Duyurulars.Find(id);
            c.Duyurulars.Remove(duyuru);
            c.SaveChanges();
            return RedirectToAction("Duyurular");
        }
        public IActionResult EditDuyuru(int id)
        {
            var duyuru = c.Duyurulars.Find(id);
            return View(duyuru);
        }
        [HttpPost]
        public async Task<IActionResult> EditDuyuru(Duyurular d, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var duyuru = c.Duyurulars.Find(d.DuyuruId);
                duyuru.DuyuruAd = d.DuyuruAd;
                duyuru.DuyuruAciklama = d.DuyuruAciklama;
                duyuru.IsHome = d.IsHome;
                //etkinlik.EtkinlikFotoLink = e.EtkinlikFotoLink;
                //if (file != null)
                //{
                //    var extention = Path.GetExtension(file.FileName);
                //    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                //    etkinlik.EtkinlikFotoLink = randomName;
                //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Etkinlikler", randomName);

                //    using (var stream = new FileStream(path, FileMode.Create))
                //    {
                //        await file.CopyToAsync(stream);
                //    }
                //}
                c.SaveChanges();
                return RedirectToAction("Duyurular");
            }
            return View(d);
        }
    }
}
