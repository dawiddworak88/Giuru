using Foundation.ApiExtensions.Models.Response;
using System.Net;

namespace Foundation.ApiExtensions.Communications
{
    public class ApiResponse<T> where T: BaseResponseModel
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Message { get; set; }
    }
}
