using Foundation.Schema.Models;
using Foundation.TenantDatabase.Shared.Contexts;
using System;

namespace Foundation.Schema.Services.SchemaServices
{
    public class SchemaService : ISchemaService
    {
        private readonly TenantDatabaseContext tenantDatabaseContext;

        public SchemaService(TenantDatabaseContext tenantDatabaseContext)
        {
            this.tenantDatabaseContext = tenantDatabaseContext;
        }

        public SchemaModel GetSchema(Guid schemaId)
        {
            var schemaModel = new SchemaModel();

            return schemaModel;
        }
    }
}
