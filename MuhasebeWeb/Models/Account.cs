using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuhasebeWeb.Models
{
    public class Account
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [InverseProperty("account")]
        public virtual ICollection<AccountCalcRelation> accountsCalcs { get; set; }

    }

}