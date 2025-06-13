using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    [Table("Trade")]
    public class Trade
    {
        [Key]
        public int TradeId { get; set; }

        [Required]
        [StringLength(30)]
        public string Account { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string AccountType { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public double? BuyQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? SellQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? BuyPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? SellPrice { get; set; }

        public DateTime? TradeDate { get; set; }

        [StringLength(125)]
        public string TradeSecurity { get; set; } = null!;

        [StringLength(10)]
        public string TradeStatus { get; set; } = null!;

        [StringLength(125)]
        public string Trader { get; set; } = null!;

        [StringLength(125)]
        public string Benchmark { get; set; } = null!;

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