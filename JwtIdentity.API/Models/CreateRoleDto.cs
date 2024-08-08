using System.ComponentModel.DataAnnotations;

namespace JwtIdentity.API.Models
{
    public class CreateRoleDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
