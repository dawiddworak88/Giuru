using Foundation.Extensions.ExtensionMethods;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Foundation.Search.Extensions
{
    public static class StringExtensions
    {
        private const string CreatedDatePropertyName = "CreatedDate";
        private const string TieBreakerPropertyName = "ProductId";

        public static SortDescriptor<T> ToElasticSortList<T>(this string orderBy) where T : class
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = typeof(T).GetProperties().Any(x => x.Name == CreatedDatePropertyName)
                    ? $"{CreatedDatePropertyName} desc"
                    : null;
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return null;
            }

            var sortDescriptor = new SortDescriptor<T>();
            var usedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var orderByClause in orderBy.Split(','))
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.InvariantCulture);
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());

                if (property == null)
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                usedProperties.Add(property.Name);

                sortDescriptor.Field(ResolveElasticField(property), orderDescending ? SortOrder.Descending : SortOrder.Ascending);
            }

            AppendTieBreaker<T>(sortDescriptor, usedProperties);

            return sortDescriptor;
        }

        private static string ResolveElasticField(PropertyInfo property)
        {
            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            var fieldName = property.Name.ToCamelCase();

            return underlyingType == typeof(string) ? $"{fieldName}.keyword" : fieldName;
        }

        private static void AppendTieBreaker<T>(SortDescriptor<T> sortDescriptor, HashSet<string> usedProperties) where T : class
        {
            if (usedProperties.Contains(TieBreakerPropertyName))
            {
                return;
            }

            var tieBreakerProperty = typeof(T).GetProperties().FirstOrDefault(x => x.Name == TieBreakerPropertyName);

            if (tieBreakerProperty == null)
            {
                return;
            }

            sortDescriptor.Field(ResolveElasticField(tieBreakerProperty), SortOrder.Ascending);
        }
    }
}
