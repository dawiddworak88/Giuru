using Foundation.Extensions.Definitions;
using System;

namespace Foundation.Extensions.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message, int statusCode) : base(message)
        {
            this.Data.Add(FilterConstants.StatusCodeKeyName, statusCode);
        }

        public CustomException(string message, int statusCode, string redirectUrl) : base(message)
        {
            this.Data.Add(FilterConstants.StatusCodeKeyName, statusCode);
            this.Data.Add(FilterConstants.RedirectUrlKeyName, redirectUrl);
        }
    }
}
