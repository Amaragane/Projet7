using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    [Table("CurvePoint")]
    public class CurvePoint
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ne dois pas être vide")]
        public byte? CurveId { get; set; }

        public DateTime? AsOfDate { get; set; }

        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}