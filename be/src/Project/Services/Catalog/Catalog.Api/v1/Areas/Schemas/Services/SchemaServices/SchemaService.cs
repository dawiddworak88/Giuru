using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;

namespace Catalog.Api.v1.Areas.Schemas.Services.SchemaServices
{
    public class SchemaService : ISchemaService
    {
        private readonly CatalogContext context;

        public SchemaService(
            CatalogContext context)
        {
            this.context = context;
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