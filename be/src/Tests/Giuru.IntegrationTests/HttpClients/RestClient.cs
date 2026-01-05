using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.HttpClients
{
    public class RestClient
    {
        private readonly HttpClient _client;

        public RestClient(HttpClient client)
        {
            _client = client;
        } 

        public async Task<T> PostAsync<S, T>(string requestUrl, S request) where S : class
        {
            var response = await _client.PostAsync(
                    requestUrl,
                    new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"POST {requestUrl} failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {JsonConvert.SerializeObject(result)}");
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException($"POST {requestUrl} returned empty body with {(int)response.StatusCode}.");
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GET {requestUrl} failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {JsonConvert.SerializeObject(result)}");
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException($"GET {requestUrl} returned empty body with {(int)response.StatusCode}.");
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
