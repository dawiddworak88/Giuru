using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure;

namespace Giuru.UnitTests.Extensions
{
    public class OrdersQueryableExtensionsTests
    {
        private static string SearchSql(string searchTerm, bool isSeller)
        {
            var options = new DbContextOptionsBuilder<OrderingContext>()
                .UseSqlServer("Server=localhost;Database=Ordering;Trusted_Connection=True;")
                .Options;

            using var context = new OrderingContext(options);

            return context.Orders
                .Where(x => x.IsActive)
                .ApplySearch(context, searchTerm, isSeller)
                .ToQueryString();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ApplySearch_WhenSearchTermEmpty_AddsNoFilter(string searchTerm)
        {
            var sql = SearchSql(searchTerm, isSeller: true);

            Assert.DoesNotContain("LIKE", sql);
            Assert.DoesNotContain("OrderItems", sql);
        }

        [Fact]
        public void ApplySearch_WhenSearchTermIsOrderId_FiltersByIdOnly()
        {
            var sql = SearchSql($" {Guid.NewGuid()} ", isSeller: true);

            Assert.Contains("[Id] = @", sql);
            Assert.DoesNotContain("LIKE", sql);
            Assert.DoesNotContain("CONVERT(", sql);
        }

        [Fact]
        public void ApplySearch_WhenSeller_SearchesClientNameAndExternalReferencePrefixWithoutFunctionsOnColumns()
        {
            var sql = SearchSql("ZAM-123", isSeller: true);

            Assert.Contains("[ClientName] LIKE", sql);
            Assert.Contains("[ExternalReferencePrefix] LIKE", sql);
            Assert.Contains("UNION", sql);
            Assert.DoesNotContain("LOWER(", sql);
            Assert.DoesNotContain("CONVERT(", sql);
        }

        [Fact]
        public void ApplySearch_WhenNotSeller_SearchesOnlyExternalReferencePrefix()
        {
            var sql = SearchSql("ZAM-123", isSeller: false);

            Assert.Contains("[ExternalReferencePrefix] LIKE", sql);
            Assert.DoesNotContain("[ClientName] LIKE", sql);
            Assert.DoesNotContain("LOWER(", sql);
        }
    }
}
