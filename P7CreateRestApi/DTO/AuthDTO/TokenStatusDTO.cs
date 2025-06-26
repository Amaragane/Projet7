namespace P7CreateRestApi.DTO.AuthDTO
{
    public class TokenStatusDTO
    {
        public bool IsValid { get; set; }
        public bool IsExpiringSoon { get; set; }
        public bool NeedsRenewal { get; set; }
    }
}
