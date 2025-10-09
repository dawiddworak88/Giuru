using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.ApiExtensions.Services.ApiClientServices
{
    public class ApiClientService : IApiClientService
    {
        public async Task<ApiResponse<T>> PostMultipartFormAsync<S, W, T>(S request) where S : ApiRequest<W> where T : BaseResponseModel where W : FileRequestModelBase
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

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(request.Language));
                }

                using (var content = new MultipartFormDataContent())
                {
                    if (request?.Data.File != null && !string.IsNullOrWhiteSpace(request.Data.Filename))
                    {
                        content.Add(new StreamContent(new MemoryStream(request.Data.File)), ApiConstants.ContentNames.FileContentName, request.Data.Filename);
                        content.Add(new StringContent(request.Language), ApiConstants.ContentNames.LanguageContentName);

                        if (request.Data.Id != null)
                        {
                            content.Add(new StringContent(request.Data.Id), ApiConstants.ContentNames.GuidContentName);
                        }

                        if (request.Data.ChunkNumber.HasValue)
                        {
                            content.Add(new StringContent(request.Data.ChunkNumber.ToString()), ApiConstants.ContentNames.ChunkNumberContentName);
                        }

                        var response = await client.PostAsync(request.EndpointAddress, content);

                        return await HandleApiResponseAsync<T>(response);
                    }
                }
            }

            return default;
        }

        public async Task<ApiResponse<T>> PostAsync<S, W, T>(S request) where S : ApiRequest<W> where T : class
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

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(request.Language));
                }

                var response = await client.PostAsync(
                    request.EndpointAddress,
                    new StringContent(JsonConvert.SerializeObject(request.Data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

                return await HandleApiResponseAsync<T>(response);
            }
        }

        public async Task<ApiResponse<T>> GetAsync<S, W, T>(S request) where S : ApiRequest<W> where T : class
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

                return await HandleApiResponseAsync<T>(response);
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<S, W, T>(S request) where S : ApiRequest<W> where T : BaseResponseModel
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

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(request.Language));
                }

                var properties = from p in request.Data.GetType().GetProperties()
                                 where p.GetValue(request.Data, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(request.Data, null).ToString());

                var queryString = string.Join("&", properties.ToArray());

                var response = await client.DeleteAsync(request.EndpointAddress + "?" + queryString);

                return await HandleApiResponseAsync<T>(response);
            }
        }

        private async Task<ApiResponse<T>> HandleApiResponseAsync<T>(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse<T>
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode
            };

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrWhiteSpace(result))
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<T>(result);
                }
            }
            else
            {
                try
                {
                    var customException = JsonConvert.DeserializeObject<CustomException>(result);

                    if (customException != null)
                    {
                        apiResponse.Message = customException.Message;
                    }
                }
                catch
                {
                    try
                    {
                        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

                        if (problemDetails != null)
                        {
                            apiResponse.Problem = problemDetails;
                        }
                    }
                    catch
                    {
                        apiResponse.RawError = result;
                    }
                }
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> PostWithQueryAsync<S, W, Q, T>(S request, Q queryObject)
            where S : ApiRequest<W>
            where T : class
        {
            var queryParams = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            if (queryObject is not null)
            {
                foreach (var prop in typeof(Q).GetProperties())
                {
                    var value = prop.GetValue(queryObject);

                    if (value is null) continue;

                    var stringValue = value switch
                    {
                        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                        bool b => b ? "true" : "false",
                        _ => value.ToString()
                    };

                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        var key = char.ToLowerInvariant(prop.Name[0]) + prop.Name[1..];

                        queryParams[key] = stringValue;
                    }
                }
            }
            var endpoint = request.EndpointAddress;

            if (queryParams is not null && queryParams.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, queryParams);
            }

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

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(request.Language));
                }

                var response = await client.PostAsync(
                    endpoint,
                    new StringContent(JsonConvert.SerializeObject(request.Data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    Encoding.UTF8,
                    "application/json"));

                return await HandleApiResponseAsync<T>(response);
            }
        }
    }
}
