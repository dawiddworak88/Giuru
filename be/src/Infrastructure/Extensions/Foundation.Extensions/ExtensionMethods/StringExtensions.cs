using Foundation.Extensions.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Extensions.ExtensionMethods
{
    public static class StringExtensions
    {
        public static IEnumerable<Guid> ToEnumerableGuidIds(this string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                return ids.Split(EndpointParameterConstants.ParameterSeparator).Select(x => Guid.Parse(x));
            }

            return default;
        }
    }
}
