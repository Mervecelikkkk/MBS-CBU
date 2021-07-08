using MezunBilgiSistemi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Models
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-746250O\\SQLEXPRESS; database=MezunBilgiSistemi; integrated security=true");
        }
        public DbSet<Etkinlikler> Etkinliklers { get; set; }
        public DbSet<Duyurular> Duyurulars { get; set; }
        public DbSet<Haberler> Haberlers { get; set; }
    }
}
