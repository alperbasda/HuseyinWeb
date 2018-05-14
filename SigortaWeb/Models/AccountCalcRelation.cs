using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SigortaWeb.Models
{
    public class AccountCalcRelation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int accountId { get; set; }

        [ForeignKey("accountId")]
        public virtual Account account { get; set; }

        public int calcId { get; set; }

        [ForeignKey("calcId")]
        public virtual Calc calc { get; set; }

    }
}