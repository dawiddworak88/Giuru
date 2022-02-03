using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Foundation.Extensions.Filters
{
    public class HttpWebGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;

        public HttpWebGlobalExceptionFilter(IWebHostEnvironment env)
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

                var statusCode = (int?)context.Exception.Data[FilterConstants.StatusCodeKeyName];

                if (((int?)context.Exception.Data[FilterConstants.StatusCodeKeyName]).HasValue)
                {
                    if (statusCode.Value == (int)HttpStatusCode.Unauthorized)
                    {
                        context.Result = new RedirectResult("/Accounts/Account/SignOutNow");
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Result = new ObjectResult(response);
                        context.HttpContext.Response.StatusCode = statusCode.Value;
                    }
                }
            }
        }

        private class ErrorResponse
        {
            public string Message { get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
