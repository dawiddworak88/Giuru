using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foundation.Extensions.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;

        private HttpGlobalExceptionFilter()
        { }

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(CustomException))
            {
                var response = new ErrorResponse
                {
                    Message = context.Exception.Message
                };

                if (this.env.EnvironmentName == EnvironmentConstants.DevelopmentEnvironmentName)
                {
                    response.DeveloperMessage = context.Exception;
                }

                if (((int?)context.Exception.Data[FilterConstants.StatusCodeKeyName]).HasValue)
                {
                    var statusCodeValue = ((int?)context.Exception.Data[FilterConstants.StatusCodeKeyName]).Value;

                    if (statusCodeValue == FilterConstants.NoContentStatusCode)
                    {
                        context.HttpContext.Response.StatusCode = statusCodeValue;
                    }
                    else
                    {
                        context.Result = new ObjectResult(response);
                        context.HttpContext.Response.StatusCode = statusCodeValue;
                    }
                }
            }
        }

        private class ErrorResponse
        {
            public string Message{ get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
