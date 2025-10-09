using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace Foundation.Extensions.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public HttpGlobalExceptionFilter(
            IWebHostEnvironment env,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _env = env;
            _globalLocalizer = globalLocalizer;
        }

        public void OnException(ExceptionContext context)
        {
            var (statusCode, message) = context.Exception switch
            {
                NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
                ConflictException ex => (StatusCodes.Status409Conflict, ex.Message),
                UnprocessableEntityException ex => (StatusCodes.Status422UnprocessableEntity, ex.Message),
                BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message),
                UnauthorizedException ex => (StatusCodes.Status401Unauthorized, ex.Message),
                CustomException ex => (ex.StatusCode, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, _globalLocalizer.GetString("AnErrorOccurred"))
            };

            context.HttpContext.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                Message = message
            };

            if (_env.IsDevelopment())
            {
                response.DeveloperMessage = context.Exception;
            }

            if (statusCode != FilterConstants.NoContentStatusCode &&
                statusCode != FilterConstants.UnauthorizedStatusCode)
            {
                context.Result = new ObjectResult(response);
            }

            context.ExceptionHandled = true;
        }

        private class ErrorResponse
        {
            public string Message{ get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
