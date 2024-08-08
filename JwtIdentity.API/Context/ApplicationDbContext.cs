using JwtIdentity.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtIdentity.API.Context
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options){}


    }
}
