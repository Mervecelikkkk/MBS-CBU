using System;
using MezunBilgiSistemi.Areas.Identity.Data;
using MezunBilgiSistemi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MezunBilgiSistemi.Areas.Identity.IdentityHostingStartup))]
namespace MezunBilgiSistemi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MBSDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MBSDbContextConnection")));

                services.AddDefaultIdentity<MBSUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MBSDbContext>();
            });
        }
    }
}