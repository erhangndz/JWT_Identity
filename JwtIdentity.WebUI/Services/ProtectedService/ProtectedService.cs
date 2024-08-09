
using Newtonsoft.Json;

namespace JwtIdentity.WebUI.Services.ProtectedService
{
    public class ProtectedService : IProtectedService
    {
        private readonly HttpClient _client;

        public ProtectedService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetInfo()
        {
            var response = await _client.GetAsync("protected");
            if(response.IsSuccessStatusCode)
            {
               var content =  await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<string>(content);
                return result;
            }

           return "Giriş başarısız";
        }
    }
}
