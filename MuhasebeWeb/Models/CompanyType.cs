using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NihaiWeb.Models;

namespace MuhasebeWeb.Models
{
    public class CompanyType
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [NotMapped]
        public virtual ICollection<Company> TypeinCompany { get; set; }
    }
}