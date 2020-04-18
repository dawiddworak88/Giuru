using Microsoft.EntityFrameworkCore;

namespace Foundation.Database.Shared.Contexts
{
    public class DatabaseContextFactory
    {
        public DatabaseContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();

            var context = new DatabaseContext(optionsBuilder.Options);

            return context;
        }
    }
}
