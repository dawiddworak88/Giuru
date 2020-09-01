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
                            // definitionValue.Add("title", taxonomy.Name);

                            var flattenedTaxonomies = this.GetFlatTaxonomyDescendants(connectionString, taxonomy.Id);

                            var definitionItems = new JArray();

                            foreach (var flattenedTaxonomy in flattenedTaxonomies)
                            {
                            }

                            definitionValue.Add("anyOf", definitionItems);
                        }
                    }
                }
            }

            return jsonSchema.ToString();
        }

        private IEnumerable<Taxonomy> GetFlatTaxonomyDescendants(string connectionString, Guid rootId)
        {
            var taxonomiesList = new List<Taxonomy>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = "DECLARE @Id uniqueidentifier = @rootid ; WITH cte AS ( SELECT a.Id, a.ParentId, a.Name, a.IsActive, a.[Order], a.LastModifiedDate, a.LastModifiedBy, a.CreatedDate, a.CreatedBy FROM Taxonomies a WHERE Id = @Id UNION ALL SELECT a.Id, a.Parentid, a.Name, a.IsActive, a.[Order], a.LastModifiedDate, a.LastModifiedBy, a.CreatedDate, a.CreatedBy FROM Taxonomies a JOIN cte c ON a.ParentId = c.Id ) SELECT ParentId, Id, Name, IsActive, [Order], LastModifiedDate, LastModifiedBy, CreatedDate, CreatedBy FROM cte WHERE ParentId is not null";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@rootId", rootId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        var taxonomyItem = new Taxonomy
                        {
                            Id = (Guid)reader["Id"],
                            Order = (int)reader["Order"],
                            IsActive = (bool)reader["IsActive"],
                            LastModifiedDate = (DateTime)reader["LastModifiedDate"],
                            CreatedDate = (DateTime)reader["CreatedDate"]
                        };

                        taxonomiesList.Add(taxonomyItem);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return taxonomiesList;
        }
    }
}