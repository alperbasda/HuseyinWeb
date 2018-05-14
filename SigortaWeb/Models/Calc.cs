using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SigortaWeb.Models
{
    public class Calc
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public bool sumAll { get; set; }

        public string formul { get; set; }

        public bool isNew { get; set; }

        [InverseProperty("calc")]
        public virtual ICollection<AccountCalcRelation> calcsAccounts { get; set; }

    }
}