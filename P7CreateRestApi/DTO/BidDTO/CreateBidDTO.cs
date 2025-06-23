namespace P7CreateRestApi.DTO.BidDTO
{
    public class CreateBidDTO
    {
        public string Account { get; set; } = null!;
        public string BidType { get; set; } = null!;
        public double? BidQuantity { get; set; }
    }
}
