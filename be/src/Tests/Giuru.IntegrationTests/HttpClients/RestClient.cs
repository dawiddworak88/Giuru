using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Giuru.IntegrationTests.HttpClients
{
    public class RestClient
    {
        private readonly HttpClient _client;

        public RestClient(HttpClient client)
        {
            _client = client;
        } 

        public async Task<RestClientResponse<T>> PostAsync<S, T>(string requestUrl, S request) where S : class
        {
            var response = await _client.PostAsync(
                    requestUrl,
                    new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            var apiResponse = new RestClientResponse<T>
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode
            };

            if (string.IsNullOrWhiteSpace(result) is false)
            {
                apiResponse.Data = JsonConvert.DeserializeObject<T>(result);
            }

            return apiResponse;
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
