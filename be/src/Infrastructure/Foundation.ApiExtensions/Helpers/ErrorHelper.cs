using Foundation.ApiExtensions.ErrorHandling;
using System;
using System.Reflection;

namespace Foundation.ApiExtensions.Helpers
{
    public static class ErrorHelper
    {
        public static Error GenerateErrorSignature()
        {
            return new Error
            {
                ErrorId = Guid.NewGuid().ToString(),
                ErrorSource = Assembly.GetExecutingAssembly().ToString()
            };
        }
    }
}
