using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.AppService.ApiApps.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace MezunBilgiSistemi.Areas.Panel.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            User _user = _userService.GetByUserNameAndPassword(user.UserName, user.Password);
            if (_user != null)
            {
                AuthenticationHelper.CreateAuthCookie(
                Guid.NewGuid(), _user.UserName,
                _user.Email,
                DateTime.Now.AddDays(30),
                _userService.GetUserRoles(_user).Select(u => u.RoleName).ToArray(),
                false,
                _user.FirstName,
                _user.LastName);
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
