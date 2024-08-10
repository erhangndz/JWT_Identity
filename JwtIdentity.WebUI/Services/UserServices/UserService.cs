using JwtIdentity.WebUI.Models;
using NuGet.DependencyResolver;
using System.IdentityModel.Tokens.Jwt;

namespace JwtIdentity.WebUI.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetAccessToken()
        {
          var result =  await _client.GetAsync("users/token");
          
              return await result.Content.ReadAsStringAsync();
          
           
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            var result = await _client.PostAsJsonAsync("users/login",loginDto);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
            
        }
    }
}
