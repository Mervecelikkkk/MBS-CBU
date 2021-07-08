using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezunBilgiSistemi.Models
{
    public class Duyurular
    {
        [Key]
        public int DuyuruId { get; set; }
        public string DuyuruAd { get; set; }
        public string DuyuruAciklama { get; set; }
        public string DuyuruFotoLink { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarih { get; set; }

        public bool IsHome { get; set; }
    }
}
