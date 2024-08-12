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

        public async Task<T> PostAsync<S, T>(string requestUrl, S request) where S : class
        {
            var response = await _client.PostAsync(
                    requestUrl,
                    new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetAsync<S, T>(string requestUrl, S request) where S : class
        {
            var properties = from p in request.GetType().GetProperties()
                             where p.GetValue(request, null) != null
                             select p.Name + "=" +
                             (p.PropertyType == typeof(DateTime) ?
                               HttpUtility.UrlEncode(((DateTime)p.GetValue(request, null)).ToString("o")) :
                               HttpUtility.UrlEncode(p.GetValue(request, null).ToString()));

            var queryString = string.Join("&", properties.ToArray());

            var response = await _client.GetAsync(requestUrl + "?" + queryString);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
            /* using (var client = new HttpClient())
             {
                 if (ApiExtensionsConstants.TimeoutMilliseconds > 0)
                 {
                     client.Timeout = TimeSpan.FromMilliseconds(ApiExtensionsConstants.TimeoutMilliseconds);
                 }

                 if (!string.IsNullOrWhiteSpace(request.AccessToken))
                 {
                     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
                 }

                 if (!string.IsNullOrWhiteSpace(request.Language))
                 {
                     client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(request.Language));
                 }

                 var properties = from p in request.Data.GetType().GetProperties()
                                  where p.GetValue(request.Data, null) != null
                                  select p.Name + "=" +
                                  (p.PropertyType == typeof(DateTime) ?
                                    HttpUtility.UrlEncode(((DateTime)p.GetValue(request.Data, null)).ToString("o")) :
                                    HttpUtility.UrlEncode(p.GetValue(request.Data, null).ToString()));

                 var queryString = string.Join("&", properties.ToArray());

                 var response = await client.GetAsync(request.EndpointAddress + "?" + queryString);

                 var apiResponse = new ApiResponse<T>
                 {
                     IsSuccessStatusCode = response.IsSuccessStatusCode,
                     StatusCode = response.StatusCode
                 };

                 var result = await response.Content.ReadAsStringAsync();

                 if (!string.IsNullOrWhiteSpace(result))
                 {
                     apiResponse.Data = JsonConvert.DeserializeObject<T>(result);
                 }

                 return apiResponse;
             }*/
        }
    }
}
