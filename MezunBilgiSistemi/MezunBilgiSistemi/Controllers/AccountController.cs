using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReportPortal.Client.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
