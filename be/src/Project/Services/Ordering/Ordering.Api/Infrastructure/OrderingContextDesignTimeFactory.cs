using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Api.Infrastructure
{
    public class OrderingContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderingContext>
    {
        public OrderingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();

            optionsBuilder.UseSqlServer("Server=.;Database=Ordering;Trusted_Connection=True;TrustServerCertificate=True;");

            return new OrderingContext(optionsBuilder.Options);
        }
    }
}
