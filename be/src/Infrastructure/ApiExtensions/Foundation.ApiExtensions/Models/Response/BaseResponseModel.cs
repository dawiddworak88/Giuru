using Foundation.ApiExtensions.ErrorHandling;

namespace Foundation.ApiExtensions.Models.Response
{
    public class BaseResponseModel
    {
        public Error Error { get; set; }
        public string Message { get; set; }
    }
}
