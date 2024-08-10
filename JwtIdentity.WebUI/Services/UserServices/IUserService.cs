using JwtIdentity.WebUI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace JwtIdentity.WebUI.Services.UserServices
{
    public interface IUserService
    {

        Task<bool> Login(LoginDto loginDto);

        Task<string> GetAccessToken();
    }
}
