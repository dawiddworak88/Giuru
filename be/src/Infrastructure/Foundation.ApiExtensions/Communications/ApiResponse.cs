using System.Net;

namespace Foundation.ApiExtensions.Communications
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
