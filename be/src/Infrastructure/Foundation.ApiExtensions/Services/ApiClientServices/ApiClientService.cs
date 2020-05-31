using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.ApiExtensions.Services.ApiClientServices
{
    public class ApiClientService : IApiClientService
    {
        public T InitializeRequestModelContext<T>(T requestModel) where T: BaseRequestModel
        {
            requestModel.Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return requestModel;
        }

        public async Task<ApiResponse<T>> PostAsync<S, W, T>(S request) where S: ApiRequest<W> where T: BaseResponseModel
        {
            using (var client = new HttpClient())
            {
                if (ApiExtensionsConstants.TimeoutMilliseconds > 0)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(ApiExtensionsConstants.TimeoutMilliseconds);
                }

                if (!string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
                }

                var response = await client.PostAsync(
                    request.EndpointAddress,
                    new StringContent(JsonConvert.SerializeObject(request.Data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

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
            }
        }
    }
}
