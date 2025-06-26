using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO.AuthDTO
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
    }
}
