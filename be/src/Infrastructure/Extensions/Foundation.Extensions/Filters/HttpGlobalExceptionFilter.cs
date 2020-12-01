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

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(CustomException))
            {
                var redirectUrl = (string)context.Exception.Data[FilterConstants.RedirectUrlKeyName];

                if (string.IsNullOrWhiteSpace(redirectUrl))
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
                        context.Result = new ObjectResult(response);
                        context.HttpContext.Response.StatusCode = ((int?)context.Exception.Data[FilterConstants.StatusCodeKeyName]).Value;
                    }
                }
                else
                {
                    context.HttpContext.Response.Redirect(redirectUrl);
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
