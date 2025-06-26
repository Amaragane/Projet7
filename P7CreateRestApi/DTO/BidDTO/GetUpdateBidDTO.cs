using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO.BidDTO
{
    public class GetUpdateBidDTO
    {
        public int BidListId { get; set; }
        [Required(ErrorMessage = "Le compte est requis")]
        [StringLength(30, ErrorMessage = "Le compte ne peut dépasser 30 caractères")]
        public string Account { get; set; } = null!;
        [Required(ErrorMessage = "Le type d'offre est requis")]
        [StringLength(30, ErrorMessage = "Le type ne peut dépasser 30 caractères")]
        public string BidType { get; set; } = null!;
        [Range(0.01, double.MaxValue, ErrorMessage = "La quantité d'offre doit être positive")]
        public double? BidQuantity { get; set; }
    }
}
