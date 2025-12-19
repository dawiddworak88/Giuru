using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.HttpClients
{
    public class TokenClient
    {
        private readonly HttpClient _httpClient;

        public TokenClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync(string tokenEndpoint)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync(tokenEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to obtain token: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            if (tokenResponse?.AccessToken is null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new Exception($"Token response missing Token field. Body: {responseContent}");
            }

            return tokenResponse.AccessToken;
        }

        private class TokenResponse
        {
            [JsonProperty("Token")]
            public string AccessToken { get; set; }
        }
    }
}
