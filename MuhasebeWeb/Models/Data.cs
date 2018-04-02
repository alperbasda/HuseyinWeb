using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MuhasebeWeb.Models;

namespace MuhasebeWeb.Models
{
    public class Data
    {
        [Key]
        [Required]
        public int id { get; set; }

        public int companyId { get; set; }

        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int branchId { get; set; }

        [ForeignKey("branchId")]
        public virtual Branch branch { get; set; }

        public int accountId { get; set; }

        [ForeignKey("accountId")]
        public virtual Account account { get; set; }

        public DateTime date { get; set; }

        public decimal amount { get; set; }

        public int CompanyTypeId { get; set; }


    }
}