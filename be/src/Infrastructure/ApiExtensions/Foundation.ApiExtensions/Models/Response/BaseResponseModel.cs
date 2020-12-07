using Foundation.ApiExtensions.ErrorHandling;
using System;

namespace Foundation.ApiExtensions.Models.Response
{
    public class BaseResponseModel
    {
        public Guid? Id { get; set; }
        public Error Error { get; set; }
        public string Message { get; set; }
    }
}
