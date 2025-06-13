using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers
{
    [Table("RuleName")]
    public class RuleName
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(125)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string Json { get; set; } = null!;

        [Required]
        [StringLength(512)]
        public string Template { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string SqlStr { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string SqlPart { get; set; } = null!;
    }
}