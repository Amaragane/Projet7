namespace P7CreateRestApi.DTO.AuthDTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}