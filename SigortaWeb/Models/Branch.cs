using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SigortaWeb.Models
{
    public class Branch
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public int branchCode { get; set; }

        public int branchGroupId { get; set; }

        [ForeignKey("branchGroupId")]
        public virtual BranchGroup branchGroup { get; set; }
    }
}