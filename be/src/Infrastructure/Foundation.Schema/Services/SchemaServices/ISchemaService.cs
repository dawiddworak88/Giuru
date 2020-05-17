using Foundation.Schema.Models;
using System;

namespace Foundation.Schema.Services.SchemaServices
{
    public interface ISchemaService
    {
        SchemaModel GetSchema(Guid schemaId);
    }
}
