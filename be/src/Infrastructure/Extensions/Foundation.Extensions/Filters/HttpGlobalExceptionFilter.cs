using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Foundation.Extensions.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CustomException ex)
            {
                context.HttpContext.Response.StatusCode = ex.StatusCode;

                var response = new ErrorResponse
                {
                    Message = ex.Message
                };

                if (_env.IsDevelopment())
                {
                    response.DeveloperMessage = ex;
                }

                if (ex.StatusCode != FilterConstants.NoContentStatusCode ||
                    ex.StatusCode != FilterConstants.UnauthorizedStatusCode)
                {
                    context.Result = new ObjectResult(response);
                }

                context.ExceptionHandled = true;
            }
        }

        private class ErrorResponse
        {
            public string Message{ get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
