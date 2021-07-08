using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.ViewModels
{
    public class EtkinlikViewModel
    {
        public int EtkinlikId { get; set; }
        public string EtkinlikAd { get; set; }
        public string EtkinlikAciklama { get; set; }
        public IFormFile EtkinlikFoto { get; set; }
        public bool Ishome { get; set; }

    }
}
