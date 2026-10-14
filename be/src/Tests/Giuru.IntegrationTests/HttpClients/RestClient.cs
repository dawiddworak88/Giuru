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
        private static readonly JsonSerializerSettings CamelCaseSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        private readonly HttpClient _client;

        public RestClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> PostForResponseAsync<S>(string requestUrl, S request, string cookieHeader = null) where S : class
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request, CamelCaseSettings),
                    Encoding.UTF8,
                    "application/json")
            };

            if (cookieHeader is not null)
            {
                message.Headers.Add("Cookie", cookieHeader);
            }

            return await _client.SendAsync(message);
        }

        public async Task<HttpResponseMessage> PostMultipartForResponseAsync(string requestUrl, MultipartFormDataContent content)
        {
            return await _client.PostAsync(requestUrl, content);
        }

        public async Task<HttpResponseMessage> GetForResponseAsync(string requestUrl)
        {
            return await _client.GetAsync(requestUrl);
        }

        public async Task<HttpResponseMessage> DeleteForResponseAsync(string requestUrl)
        {
            return await _client.DeleteAsync(requestUrl);
        }

        public async Task<T> PostAsync<S, T>(string requestUrl, S request) where S : class
        {
            var response = await PostForResponseAsync(requestUrl, request);

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

        /// <summary>Asserts the status code and, when it is not the expected one, fails with the body so the cause can be read from the report.</summary>
        public static async Task AssertStatusAsync(HttpResponseMessage response, System.Net.HttpStatusCode expected)
        {
            if (response.StatusCode != expected)
            {
                var body = await response.Content.ReadAsStringAsync();

                Xunit.Assert.Fail($"Expected {expected} but got {(int)response.StatusCode} {response.StatusCode} from {response.RequestMessage?.Method} {response.RequestMessage?.RequestUri}. Body: {(body.Length > 2500 ? body.Substring(0, 2500) : body)}");
            }
        }

        /// <summary>Reads the body of a response that has already been checked, as the given type.</summary>
        public static async Task<T> ReadAsync<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>The <c>message</c> of an error body, which is what the web apps show the user.</summary>
        public static async Task<string> ReadMessageAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeAnonymousType(result, new { Message = default(string) })?.Message;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
