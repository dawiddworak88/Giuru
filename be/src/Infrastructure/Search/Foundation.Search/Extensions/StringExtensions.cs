using Foundation.Extensions.ExtensionMethods;
using Nest;
using System;
using System.Linq;

namespace Foundation.Search.Extensions
{
    public static class StringExtensions
    {
        public static SortDescriptor<T> ToElasticSortList<T>(this string orderBy) where T : class
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return null;
            }

            var sortDescriptor = new SortDescriptor<T>();

            foreach (var orderByClause in orderBy.Split(','))
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

                var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());

                if (property != null)
                {
                    sortDescriptor.Field(
                    property.PropertyType == typeof(DateTime) ? $"{property.Name.ToCamelCase()}" : $"{property.Name.ToCamelCase()}.keyword",
                    orderDescending ? SortOrder.Descending : SortOrder.Ascending);
                }
            }

            return sortDescriptor;
        }
    }
}
