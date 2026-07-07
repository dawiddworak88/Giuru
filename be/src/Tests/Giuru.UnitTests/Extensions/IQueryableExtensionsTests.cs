using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Extensions.ExtensionMethods;

namespace Giuru.UnitTests.Extensions
{
    public class IQueryableExtensionsTests
    {
        private class WithCreatedDate
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private class WithoutCreatedDate
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        private static IQueryable<WithCreatedDate> BuildDatedSource()
        {
            return new List<WithCreatedDate>
            {
                new() { Name = "oldest", CreatedDate = new DateTime(2020, 1, 1) },
                new() { Name = "newest", CreatedDate = new DateTime(2024, 1, 1) },
                new() { Name = "middle", CreatedDate = new DateTime(2022, 1, 1) },
            }.AsQueryable();
        }

        [Fact]
        public void ApplySort_WhenOrderByEmpty_AndTypeHasCreatedDate_SortsByCreatedDateDescending()
        {
            var result = BuildDatedSource().ApplySort(null).ToList();

            Assert.Equal(new[] { "newest", "middle", "oldest" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_WhenOrderByEmpty_AndTypeWithoutCreatedDate_ReturnsSourceUnsorted()
        {
            var source = new List<WithoutCreatedDate>
            {
                new() { Name = "b" },
                new() { Name = "a" },
                new() { Name = "c" },
            }.AsQueryable();

            var result = source.ApplySort(string.Empty).ToList();

            Assert.Equal(new[] { "b", "a", "c" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_WhenOrderByEmpty_AndExplicitDefaultProvided_UsesExplicitDefault()
        {
            var result = BuildDatedSource()
                .ApplySort(null, $"{nameof(WithCreatedDate.Name)} asc")
                .ToList();

            Assert.Equal(new[] { "middle", "newest", "oldest" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_WhenExplicitOrderByProvided_IgnoresDefaultAndCreatedDate()
        {
            var result = BuildDatedSource()
                .ApplySort($"{nameof(WithCreatedDate.Name)} asc", $"{nameof(WithCreatedDate.CreatedDate)} desc")
                .ToList();

            Assert.Equal(new[] { "middle", "newest", "oldest" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_WhenSourceNull_Throws()
        {
            IQueryable<WithCreatedDate> source = null;

            Assert.Throws<ArgumentNullException>(() => source.ApplySort(null));
        }
    }
}
