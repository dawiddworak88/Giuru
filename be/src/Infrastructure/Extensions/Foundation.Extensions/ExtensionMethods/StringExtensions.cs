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

        public static IEnumerable<string> ToEnumerableString(this string values)
        {
            if (!string.IsNullOrWhiteSpace(values))
            {
                return values.Split(EndpointParameterConstants.ParameterSeparator);
            }

            return default;
        }

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str[1..];
            }
            return str;
        }
    }
}
