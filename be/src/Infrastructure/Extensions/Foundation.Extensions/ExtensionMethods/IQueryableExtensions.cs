using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Foundation.Extensions.ExtensionMethods
{
    public static class IQueryableExtensions
    {
        private const string CreatedDatePropertyName = "CreatedDate";
        private const string TieBreakerPropertyName = "Id";

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            return source.ApplySort(orderBy, null);
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, string defaultOrderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = !string.IsNullOrWhiteSpace(defaultOrderBy)
                    ? defaultOrderBy
                    : GetDefaultCreatedDateOrderBy<T>();
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;
            var usedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.InvariantCulture);
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                var propertyMappingValue = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());

                if (propertyMappingValue == null)
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                usedProperties.Add(propertyMappingValue.Name);

                orderByString = orderByString +
                    (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                    + propertyMappingValue.Name
                    + (orderDescending ? " descending" : " ascending");
            }

            orderByString = EnsureStableTieBreaker<T>(orderByString, usedProperties);

            return source.OrderBy(orderByString);
        }

        private static string GetDefaultCreatedDateOrderBy<T>()
        {
            var hasCreatedDate = typeof(T).GetProperties().Any(x => x.Name == CreatedDatePropertyName);

            return hasCreatedDate ? $"{CreatedDatePropertyName} desc" : null;
        }

        private static string EnsureStableTieBreaker<T>(string orderByString, HashSet<string> usedProperties)
        {
            if (usedProperties.Contains(TieBreakerPropertyName))
            {
                return orderByString;
            }

            var tieBreakerProperty = typeof(T).GetProperties().FirstOrDefault(x => x.Name == TieBreakerPropertyName);

            if (tieBreakerProperty == null)
            {
                return orderByString;
            }

            return $"{orderByString}, {tieBreakerProperty.Name} ascending";
        }
    }
}
