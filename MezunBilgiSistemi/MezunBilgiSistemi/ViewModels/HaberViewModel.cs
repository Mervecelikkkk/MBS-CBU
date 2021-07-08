using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.ViewModels
{
    public class HaberViewModel
    {
        public int HaberID { get; set; }
        public string HaberAd { get; set; }
        public string HaberAciklama { get; set; }
        public IFormFile HaberImage { get; set; }
        public bool Ishome { get; set; }

    }
}
