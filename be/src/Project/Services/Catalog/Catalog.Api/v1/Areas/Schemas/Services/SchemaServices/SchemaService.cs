using Foundation.GenericRepository.Services;
using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;
using System.Data.SqlClient;

namespace Catalog.Api.v1.Areas.Schemas.Services.SchemaServices
{
    public class SchemaService : ISchemaService
    {
        private readonly CatalogContext context;
        private readonly IEntityService entityService;

        public SchemaService(
            CatalogContext context,
            IEntityService entityService
            )
        {
            this.context = context;
            this.entityService = entityService;
        }

        public async Task<SchemaResultModel> CreateAsync(CreateSchemaModel model)
        {
            return default;
        }


        public async Task<SchemaResultModel> GetByIdAsync(GetSchemaModel getSchemaModel)
        {
            return default;
        }

        public async Task<SchemaResultModel> GetByEntityTypeIdAsync(GetSchemaByEntityTypeModel getSchemaModel)
        {
            return default;
        }

        private async Task<string> GetJsonSchemaAsync(string jsonSchemaSerialized, string language, string connectionString = null)
        {
            var jsonSchema = JObject.Parse(jsonSchemaSerialized);

            var properties = (JObject)jsonSchema["properties"];

            var definitons = (JObject)jsonSchema["definitions"];

            if (definitons != null && definitons.Children().Any())
            {
                foreach (var definition in definitons)
                {
                    Guid definitionKeyId;

                    var isDefinitionKeyGuid = Guid.TryParse(definition.Key, out definitionKeyId);

                    if (isDefinitionKeyGuid)
                    {
                        var taxonomy = this.context.Taxonomies.FirstOrDefault(x => x.Id == definitionKeyId && x.IsActive);

                        if (taxonomy != null)
                        {
                            var definitionValue = (JObject)definition.Value;

                            definitionValue.Add("type", "string");
                        }
                    }
                }
            }

            return jsonSchema.ToString();
        }
    }
}