using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuhasebeWeb.Models
{
    public class Branch
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public int branchCode { get; set; }

        public int branchGroupId { get; set; }

        [ForeignKey("branchGroupId")]
        public virtual BranchGroup branchGroup { get; set; }
    }
}