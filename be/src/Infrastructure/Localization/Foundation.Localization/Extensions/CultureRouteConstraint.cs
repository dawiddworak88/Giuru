using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Foundation.Localization.Extensions
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "signin-oidc",
        "hc",
        "liveness"
    };

        private readonly Regex _regex;

        public CultureRouteConstraint()
        {
            // Twój oryginalny regex: en, pl-PL, itd.
            _regex = new Regex(@"^[a-zA-Z]{2}(\-[a-zA-Z]{2})?$", RegexOptions.Compiled);
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var value))
                return false;

            var culture = value?.ToString();
            if (string.IsNullOrWhiteSpace(culture))
                return false;

            // Wykluczone ścieżki
            if (ExcludedPaths.Contains(culture))
                return false;

            // Sprawdź regex
            return _regex.IsMatch(culture);
        }
    }
}
