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

        private class WithCreatedDateNoId
        {
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private static Guid Id(int n) => new($"00000000-0000-0000-0000-{n:D12}");

        private static IQueryable<WithCreatedDate> BuildDatedSource()
        {
            return new List<WithCreatedDate>
            {
                new() { Id = Id(1), Name = "oldest", CreatedDate = new DateTime(2020, 1, 1) },
                new() { Id = Id(2), Name = "newest", CreatedDate = new DateTime(2024, 1, 1) },
                new() { Id = Id(3), Name = "middle", CreatedDate = new DateTime(2022, 1, 1) },
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
                new() { Id = Id(2), Name = "b" },
                new() { Id = Id(1), Name = "a" },
                new() { Id = Id(3), Name = "c" },
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

        [Fact]
        public void ApplySort_WhenDuplicateCreatedDate_TieBreaksByIdAscending()
        {
            var sameDate = new DateTime(2024, 1, 1);
            var source = new List<WithCreatedDate>
            {
                new() { Id = Id(2), Name = "second", CreatedDate = sameDate },
                new() { Id = Id(1), Name = "first", CreatedDate = sameDate },
            }.AsQueryable();

            var result = source.ApplySort(null).ToList();

            Assert.Equal(new[] { "first", "second" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_WhenExplicitSortHasDuplicateKey_TieBreaksByIdAscending()
        {
            var source = new List<WithCreatedDate>
            {
                new() { Id = Id(2), Name = "dup", CreatedDate = new DateTime(2020, 1, 1) },
                new() { Id = Id(1), Name = "dup", CreatedDate = new DateTime(2020, 1, 1) },
                new() { Id = Id(3), Name = "aaa", CreatedDate = new DateTime(2020, 1, 1) },
            }.AsQueryable();

            var result = source.ApplySort($"{nameof(WithCreatedDate.Name)} asc").ToList();

            Assert.Equal(new[] { Id(3), Id(1), Id(2) }, result.Select(x => x.Id));
        }

        [Fact]
        public void ApplySort_WhenExplicitSortById_DoesNotBreakOrderingAndDoesNotThrow()
        {
            // Sorting by the tie-breaker itself must not be duplicated / override the requested direction.
            var result = BuildDatedSource().ApplySort("id desc").ToList();

            Assert.Equal(new[] { Id(3), Id(2), Id(1) }, result.Select(x => x.Id));
        }

        [Fact]
        public void ApplySort_WhenTypeHasNoId_AppliesNoTieBreaker_AndDoesNotThrow()
        {
            var source = new List<WithCreatedDateNoId>
            {
                new() { Name = "oldest", CreatedDate = new DateTime(2020, 1, 1) },
                new() { Name = "newest", CreatedDate = new DateTime(2024, 1, 1) },
            }.AsQueryable();

            var result = source.ApplySort(null).ToList();

            Assert.Equal(new[] { "newest", "oldest" }, result.Select(x => x.Name));
        }

        [Fact]
        public void ApplySort_PaginationOverTiedData_NoOverlapAndFullCoverage()
        {
            var sameDate = new DateTime(2024, 1, 1);
            var source = Enumerable.Range(1, 6)
                .Select(n => new WithCreatedDate { Id = Id(n), Name = $"item{n}", CreatedDate = sameDate })
                .Reverse() // shuffle input order
                .AsQueryable();

            var sorted = source.ApplySort(null);

            var page1 = sorted.Skip(0).Take(3).Select(x => x.Id).ToList();
            var page2 = sorted.Skip(3).Take(3).Select(x => x.Id).ToList();

            Assert.Empty(page1.Intersect(page2));
            Assert.Equal(6, page1.Concat(page2).Distinct().Count());
        }
    }
}
