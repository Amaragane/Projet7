using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    [Table("CurvePoint")]
    public class CurvePoint
    {
        [Key]
        public int Id { get; set; }

        public byte? CurveId { get; set; }

        public DateTime? AsOfDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? Term { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double? CurvePointValue { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}