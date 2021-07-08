using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Models
{
    public class Etkinlikler
    {
        [Key]
        public int EtkinlikId { get; set; }
        public string EtkinlikAd { get; set; }
        public string EtkinlikAciklama { get; set; }
        public string  EtkinlikFotoLink { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarih { get; set; }

        public bool IsHome { get; set; }
    }
}
