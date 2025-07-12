using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Foundation.ApiExtensions.Communications
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Message { get; set; }
        public ProblemDetails Problem { get; set; }
        public string RawError { get; set; }
    }
}
