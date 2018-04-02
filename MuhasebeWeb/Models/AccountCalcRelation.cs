using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NihaiWeb.Models;

namespace MuhasebeWeb.Models
{
    public class AccountCalcRelation
    {
        [Key]
        [Required]
        public int id { get; set; }

        public int accountId { get; set; }

        [ForeignKey("accountId")]
        public virtual Account account { get; set; }

        public int calcId { get; set; }

        [ForeignKey("calcId")]
        public virtual Calc calc { get; set; }

    }
}