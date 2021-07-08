using MezunBilgiSistemi.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi
{
    public class IdentityRole
    {
        public static void OlusturAdmin(UserManager<MBSUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            MBSUser user = new MBSUser
            {
                FirsName="Ferhat",
                LastName="Kaplan",
                UserName="Fero"
            };
            if(userManager.FindByNameAsync("Ferhat").Result==null)
            {
                var idnetityuser= userManager.CreateAsync(user,"123").Result;
            }
            if (roleManager.FindByNameAsync("admin").Result==null)
            {
                IdentityRole role = new IdentityRole
                {
                    Name="Admin"
                };
                var identityuser = roleManager.CreateAsync(role).Result;

                userManager.AddToRoleAsync(user,role.Name)
            }
        }
    }
}
