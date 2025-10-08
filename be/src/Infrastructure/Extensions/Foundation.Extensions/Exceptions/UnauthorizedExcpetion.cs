using System;

namespace Foundation.Extensions.Exceptions
{
    public class UnauthorizedExcpetion : Exception
    {
        public UnauthorizedExcpetion(string message) : base(message) { }
    }
}
