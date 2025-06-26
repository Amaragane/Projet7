using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO.UsersDTO
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(125, ErrorMessage = "Full name cannot exceed 125 characters")]
        public string Fullname { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "At least one role is required")]
        public IList<string> Roles { get; set; }= null! ;

        public bool EmailConfirmed { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
