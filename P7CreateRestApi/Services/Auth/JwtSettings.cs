namespace P7CreateRestApi.Services.Auth
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpiryInMinutes { get; set; } = 60;
        public int RefreshTokenExpiryInDays { get; set; } = 7;
    }
}