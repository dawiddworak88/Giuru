using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Analytics.Api.Shared.Configurations;

namespace Analytics.Api.Shared.Repositories.AccessToken
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly IOptions<AppSettings> _options;

        public AccessTokenRepository(
            IOptions<AppSettings> options)
        {
            _options = options;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"{_options.Value.GiuruTokenUrl}",
                    new StringContent(
                        JsonConvert.SerializeObject(new { Email = _options.Value.GiuruApiEmail, AppSecret = _options.Value.GiuruApiAppSecret, OrganisationId = _options.Value.GiuruApiOrganisationId }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                    ), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return default;
            }
        }
    }
}
