using Foundation.TenantDatabase.Shared.Contexts;
using System.Threading.Tasks;

namespace Foundation.Schema.Services.SchemaServices
{
    public class SchemaServiceFactory
    {
        private readonly TenantDatabaseContextFactory tenantDatabaseContextFactory;

        public SchemaServiceFactory(TenantDatabaseContextFactory tenantDatabaseContextFactory)
        {
            this.tenantDatabaseContextFactory = tenantDatabaseContextFactory;
        }

        public virtual async Task<ISchemaService> CreateSchemaService(string connectionString)
        {
            return new SchemaService(await this.tenantDatabaseContextFactory.CreateDbContextAsync(connectionString));
        }
    }
}
