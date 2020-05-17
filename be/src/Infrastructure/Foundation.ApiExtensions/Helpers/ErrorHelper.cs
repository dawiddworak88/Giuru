using Foundation.ApiExtensions.ErrorHandling;
using System;

namespace Foundation.ApiExtensions.Helpers
{
    public static class ErrorHelper
    {
        public static Error GenerateErrorSignature(string executingAssembly)
        {
            return new Error
            {
                ErrorId = Guid.NewGuid().ToString(),
                ErrorSource = executingAssembly
            };
        }
    }
}
