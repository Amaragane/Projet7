using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    [Table("Users")]

    public class User : IdentityUser
    {
        [Required]
        [StringLength(125)]
        public string Fullname { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; } = null;

        [Required]
        [StringLength(50)]
        public IList<string> Roles { get; set; } = new List<string> { "User" };

        public bool IsActive { get; set; } = true;
    }
}