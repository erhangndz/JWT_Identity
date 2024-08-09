using JwtIdentity.API.Models;
using System.IdentityModel.Tokens.Jwt;

namespace JwtIdentity.API.Services.UserServices
{
    public interface IUserService
    {
        Task<string> RegisterAsync(CreateUserDto createUserDto);
        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(CreateRoleDto createRoleDto);

        Task<JwtSecurityToken> GetAccessToken(AppUser appUser);
    }
}
