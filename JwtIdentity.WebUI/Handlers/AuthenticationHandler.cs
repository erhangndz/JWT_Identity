
using JwtIdentity.WebUI.Models;
using JwtIdentity.WebUI.Services.UserServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace JwtIdentity.WebUI.Handlers
{
    public class AuthenticationHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;

        public AuthenticationHandler(IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _contextAccessor = contextAccessor;
            _userService = userService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _userService.GetAccessToken();

            var accessToken = EncodeNonAsciiCharacters(token);

            

            //var accessToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response =await base.SendAsync(request, cancellationToken);
            if(response.StatusCode== System.Net.HttpStatusCode.Unauthorized)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            return response;
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
