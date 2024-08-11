
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

           return "Giriş başarısız";
        }

        static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII  
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
