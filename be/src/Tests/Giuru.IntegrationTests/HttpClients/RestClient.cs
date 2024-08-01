using System.Net.Http;
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

    }
}
