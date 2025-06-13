using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(125)]
        public string MoodysRating { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string SandPRating { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string FitchRating { get; set; } = null!;

        public byte? OrderNumber { get; set; }
    }
}