using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    [Table("Users")]

    public class User : IdentityUser
    {
        int id { get; set; }
        override public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(125)]
        public string Fullname { get; set; } = null!;
        [NotMapped]
        [Required]
        [StringLength(50)]
        public IList<string> Roles { get; set; } = new List<string> { "User" };

    }
}