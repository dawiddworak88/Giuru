using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Foundation.Extensions.ExtensionMethods
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.InvariantCulture);
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!typeof(T).GetProperties().Any(x => x.Name.ToLowerInvariant() == propertyName.ToLowerInvariant()))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                orderByString = orderByString +
                    (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                    + propertyMappingValue.Name
                    + (orderDescending ? " descending" : " ascending");
            }

            return source.OrderBy(orderByString);
        }
    }
}