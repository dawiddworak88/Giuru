using System.Collections.Generic;

namespace Foundation.ApiExtensions.Communications
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
}
