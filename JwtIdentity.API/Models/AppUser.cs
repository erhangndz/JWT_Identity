using Microsoft.AspNetCore.Identity;

namespace JwtIdentity.API.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
