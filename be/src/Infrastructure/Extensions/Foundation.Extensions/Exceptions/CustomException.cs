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
    }
}
