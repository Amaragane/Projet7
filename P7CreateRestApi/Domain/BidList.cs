using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [Column(TypeName = "decimal(10,2)")]
        public double? BidQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? AskQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? Bid { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? Ask { get; set; }

        [StringLength(125)]
        public string Benchmark { get; set; } = null!;

        public DateTime? BidListDate { get; set; }

        [StringLength(125)]
        public string Commentary { get; set; } = null!;

        [StringLength(125)]
        public string BidSecurity { get; set; } = null!;

        [StringLength(10)]
        public string BidStatus { get; set; } = null!;

        [StringLength(125)]
        public string Trader { get; set; } = null!;

        [StringLength(125)]
        public string Book { get; set; } = null!;

        [StringLength(125)]
        public string CreationName { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        [StringLength(125)]
        public string RevisionName { get; set; } = null!;

        public DateTime? RevisionDate { get; set; }

        [StringLength(125)]
        public string DealName { get; set; } = null!;

        [StringLength(125)]
        public string DealType { get; set; } = null!;

        [StringLength(125)]
        public string SourceListId { get; set; } = null!;

        [StringLength(125)]
        public string Side { get; set; } = null!;
    }
}