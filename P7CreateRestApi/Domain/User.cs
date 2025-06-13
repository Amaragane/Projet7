using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(125)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string Fullname { get; set; } = null!;

        [Required]
        [StringLength(125)]
        public string Role { get; set; } = null!;
    }
}