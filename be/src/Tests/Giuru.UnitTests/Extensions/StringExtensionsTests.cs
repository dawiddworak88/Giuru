using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Search.Extensions;
using Nest;

namespace Giuru.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        private class ElasticStub
        {
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? PublishedDate { get; set; }
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public bool IsActive { get; set; }
        }

        private class ElasticStubWithoutProductId
        {
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private class ElasticStubWithoutCreatedDate
        {
            public string Name { get; set; }
        }

        private static List<(string Field, SortOrder? Order)> Extract<T>(SortDescriptor<T> descriptor) where T : class
        {
            var sorts = ((IPromise<IList<ISort>>)descriptor).Value;

            return sorts.Cast<FieldSort>().Select(s => (s.Field.Name, s.Order)).ToList();
        }

        [Fact]
        public void ToElasticSortList_WhenEmptyOrderBy_AndTypeHasCreatedDate_DefaultsToCreatedDateDescThenProductIdAsc()
        {
            var result = Extract(string.Empty.ToElasticSortList<ElasticStub>());

            Assert.Equal(new[] { ("createdDate", (SortOrder?)SortOrder.Descending), ("productId", SortOrder.Ascending) }, result);
        }

        [Fact]
        public void ToElasticSortList_WhenStringField_UsesKeywordSubField_AndAppendsProductIdTieBreaker()
        {
            var result = Extract("name asc".ToElasticSortList<ElasticStub>());

            Assert.Equal(new[] { ("name.keyword", (SortOrder?)SortOrder.Ascending), ("productId", SortOrder.Ascending) }, result);
        }

        [Fact]
        public void ToElasticSortList_WhenNonStringFields_DoNotUseKeyword()
        {
            var result = Extract("createdDate desc, publishedDate asc, quantity asc, isActive asc"
                .ToElasticSortList<ElasticStub>());

            Assert.DoesNotContain(result, x => x.Field.EndsWith(".keyword"));
            Assert.Equal(new[] { "createdDate", "publishedDate", "quantity", "isActive", "productId" }, result.Select(x => x.Field));
        }

        [Fact]
        public void ToElasticSortList_WhenAlreadySortedByProductId_DoesNotDuplicateTieBreaker()
        {
            var result = Extract("productId asc".ToElasticSortList<ElasticStub>());

            Assert.Equal(new[] { ("productId", (SortOrder?)SortOrder.Ascending) }, result);
        }

        [Fact]
        public void ToElasticSortList_WhenTypeHasNoProductId_AppliesNoTieBreaker()
        {
            var result = Extract("name asc".ToElasticSortList<ElasticStubWithoutProductId>());

            Assert.Equal(new[] { ("name.keyword", (SortOrder?)SortOrder.Ascending) }, result);
        }

        [Fact]
        public void ToElasticSortList_WhenEmptyOrderBy_AndTypeWithoutCreatedDate_ReturnsNull()
        {
            var result = string.Empty.ToElasticSortList<ElasticStubWithoutCreatedDate>();

            Assert.Null(result);
        }
    }
}
