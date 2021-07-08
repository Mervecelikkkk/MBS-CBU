using MezunBilgiSistemi.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Models
{
    public class AnaSayfaViewModel
    {
        public IEnumerable<Duyurular> duyuruview { get; set; }
        public IEnumerable<MBSUser> usersview { get; set; }
    }
}
