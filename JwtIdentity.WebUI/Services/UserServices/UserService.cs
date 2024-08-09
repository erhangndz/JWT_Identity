using JwtIdentity.WebUI.Models;

namespace JwtIdentity.WebUI.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<TokenResponse> Login(LoginDto loginDto)
        {
            var result = await _client.PostAsJsonAsync("users/login",loginDto);
           return await result.Content.ReadFromJsonAsync<TokenResponse>();
        }
    }
}
