using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.ViewModels
{
    public class DuyuruViewModel
    {
        public int DuyuruId { get; set; }
        public string DuyuruAd { get; set; }
        public string DuyuruAciklama { get; set; }
        public IFormFile DuyuruImage { get; set; }
        public bool Ishome { get; set; }
    }
}
