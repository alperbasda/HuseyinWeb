using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SigortaWeb.Models
{
    public class Sayac
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Yillik { get; set; }

        public DateTime YillikTarih { get; set; }

        public int Aylik { get; set; }

        public DateTime AylikTarih { get; set; }

        public int Gunluk { get; set; }

        public DateTime GunlukTarih { get; set; }

        public int Ziyaret { get; set; }
    }
}