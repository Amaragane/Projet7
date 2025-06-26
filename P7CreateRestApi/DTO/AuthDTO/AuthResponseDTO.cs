using P7CreateRestApi.DTO.UsersDTO;

namespace P7CreateRestApi.DTO.AuthDTO
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAt { get; set; }
        public UserDTO User { get; set; } = null!;
        public IList<string> Permissions { get; set; } = new List<string>();
    }
}
