using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.DTO.UsersDTO
{
    public class AssignRoleDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = null!;
    }

    public class RemoveRoleDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = null!;
    }

    public class UserRolesDTO
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IList<string> CurrentRoles { get; set; } = new List<string>();
        public IList<string> AvailableRoles { get; set; } = new List<string>();
    }
}
