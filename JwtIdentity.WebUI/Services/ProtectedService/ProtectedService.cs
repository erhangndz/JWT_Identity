
using JwtIdentity.WebUI.Services.UserServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using static JwtIdentity.WebUI.Models.TokenResponse;
using System.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace JwtIdentity.WebUI.Services.ProtectedService
{
    public class ProtectedService : IProtectedService
    {
        private readonly HttpClient _client;
        private readonly IUserService _userService;

        public ProtectedService(HttpClient client, IUserService userService)
        {
            _client = client;
            _userService = userService;
        }

        public async Task<string> GetInfo()
        {
            
            

            var response = await _client.GetAsync("protected");
            if(response.IsSuccessStatusCode)
            {
               var content =  await response.Content.ReadAsStringAsync();
               
                return content;
            }

            return null;
        }

        
    }
}
