using MezunBilgiSistemi.Areas.Identity.Data;
using MezunBilgiSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Context c = new Context();
        private UserManager<MBSUser> userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<MBSUser> _userManager)
        {
            _logger = logger;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            var duyurular = c.Duyurulars.Where(d => d.IsHome == true).ToList();
            var Users = userManager.Users.Where(u => u.IsHome == true).ToList();

            AnaSayfaViewModel v = new AnaSayfaViewModel();
            v.duyuruview = duyurular;
            v.usersview = Users;
            return View(v);
        }
        //public IActionResult KullaniciGöster()
        //{
        //    var users = userManager.Users;
        //    return View(users);
        //}
        public IActionResult Hakkımızda()
        {
            return View();
        }
        [Authorize]
        public IActionResult DuyuruDetay(int? id)
        {
            var duyuru = c.Duyurulars.FirstOrDefault(x => x.DuyuruId == id);
            return View(duyuru);
        }
        [Authorize]
        public IActionResult EtkinlikDetay(int? id)
        {
            var etkinlik = c.Etkinliklers.FirstOrDefault(x => x.EtkinlikId == id);
            return View(etkinlik);
        }
        [Authorize]
        public IActionResult isveStajOlanak()
        {
            return View();
        }
        [Authorize]
        public IActionResult isveStajOlanakDetay()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> BasariHikayeleri(string id)
        {
            var users = userManager.Users.FirstOrDefault(x => x.Id == id);
            return View(users);
        }

        [Authorize]
        public IActionResult Etkinlikler()
        {
            var etkinlikler = c.Etkinliklers.ToList();
            return View(etkinlikler);
        }
        [Authorize]
        public IActionResult Duyurular()
        {
            var duyurular = c.Duyurulars.ToList();
            return View(duyurular);
        }
        //public IActionResult StajOlanakları()
        //{
        //    return View();
        //}
        public IActionResult Iletisim()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
