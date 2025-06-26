using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO.CurveDTO
{
    public class GetUpdateCurvePointDTO
    {
        public int Id { get; set; }
        [Range(1, 255, ErrorMessage = "L'ID de courbe doit être entre 1 et 255")]
        public byte? CurveId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Le terme doit être positif")]
        public double? Term { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "La valeur doit être positive")]
        public double? CurvePointValue { get; set; }
    }
}
