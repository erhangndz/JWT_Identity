using JwtIdentity.WebUI.Models;

namespace JwtIdentity.WebUI.Services.UserServices
{
    public interface IUserService
    {

        Task<TokenResponse> Login(LoginDto loginDto);
    }
}
