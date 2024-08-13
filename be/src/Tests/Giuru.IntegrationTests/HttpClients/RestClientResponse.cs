using System.Net;

namespace Giuru.IntegrationTests.HttpClients
{
    public class RestClientResponse<T> 
    {
        public bool IsSuccessStatusCode { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
}
