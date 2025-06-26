using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Dot.Net.WebApi.Domain
{
    [Table("BidList")]
    public class BidList
    {
        [Key]
        public int BidListId { get; set; }

        [Required]
        [StringLength(30)]
        public string Account { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string BidType { get; set; } = null!;

        [Range(0.01, 1000000.00, ErrorMessage = "Price must be between 0.01 and 1,000,000.00")]
        public double? BidQuantity { get; set; }

        [Range(0.01, 1000000.00, ErrorMessage = "Price must be between 0.01 and 1,000,000.00")]
        public double? AskQuantity { get; set; }

        [Range(0.01, 1000000.00, ErrorMessage = "Price must be between 0.01 and 1,000,000.00")]
        public double? Bid { get; set; }

        [Range(0.01, 1000000.00, ErrorMessage = "Price must be between 0.01 and 1,000,000.00")]
        public double? Ask { get; set; }
        [Required]
        [StringLength(125)]
        public string Benchmark { get; set; } = null!;

        public DateTime? BidListDate { get; set; }
        [Required]
        [StringLength(125)]
        public string Commentary { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string BidSecurity { get; set; } = null!;
        [Required]
        [StringLength(10)]
        public string BidStatus { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string Trader { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string Book { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string CreationName { get; set; } = null!;

        public DateTime? CreationDate { get; set; }
        [Required]
        [StringLength(125)]
        public string RevisionName { get; set; } = null!;

        public DateTime? RevisionDate { get; set; }
        [Required]
        [StringLength(125)]
        public string DealName { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string DealType { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string SourceListId { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string Side { get; set; } = null!;
    }

}