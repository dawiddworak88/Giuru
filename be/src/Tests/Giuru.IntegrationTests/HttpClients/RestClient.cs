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

            if (string.IsNullOrWhiteSpace(result) is false)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }

            return default;
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);

            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result) is false)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }

            return default;
        }
    }
}
