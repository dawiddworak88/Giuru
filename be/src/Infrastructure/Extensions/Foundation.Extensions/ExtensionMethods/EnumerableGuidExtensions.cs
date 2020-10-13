using Foundation.Extensions.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Extensions.ExtensionMethods
{
    public static class EnumerableGuidExtensions
    {
        public static string ToEndpointParameterString(this IEnumerable<Guid> ids)
        {
            if (ids != null && ids.Any())
            {
                return string.Join(EndpointParameterConstants.ParameterSeparator, ids);
            }

            return default;
        }
        
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
