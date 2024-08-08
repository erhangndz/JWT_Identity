using JwtIdentity.API.Models;

namespace JwtIdentity.API.Services.UserServices
{
    public interface IUserService
    {
        Task<string> RegisterAsync(CreateUserDto createUserDto);
        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(CreateRoleDto createRoleDto);
    }
}
