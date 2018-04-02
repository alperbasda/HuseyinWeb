using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuhasebeWeb.Models
{
    public class Company
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public int companyTypeId { get; set; }

        [ForeignKey("companyTypeId")]
        public virtual CompanyType companyType { get; set; }



    }
}