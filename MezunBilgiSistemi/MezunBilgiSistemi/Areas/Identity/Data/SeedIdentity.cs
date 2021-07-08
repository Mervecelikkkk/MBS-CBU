using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Areas.Identity.Data
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<MBSUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            var username = configuration["Data:AdminUser:username"];
            var password = configuration["Data:AdminUser:password"];
            var email = configuration["Data:AdminUser:email"];
            var role = configuration["Data:AdminUser:role"];

            if (await userManager.FindByNameAsync(username)==null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));

                var user = new MBSUser()
                {
                    UserName = username,
                    Email = email,
                    FirsName = "Ferhat",
                    LastName = "Kaplan",
                    EmailConfirmed = true
                };


                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
