using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Models
{
    public class Haberler
    {
        [Key]
        public int HaberID { get; set; }
        public string HaberAd { get; set; }
        public string HaberAciklama { get; set; }
        public string HaberFotoLink { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarih { get; set; }

        public bool IsHome { get; set; }
    }
}
