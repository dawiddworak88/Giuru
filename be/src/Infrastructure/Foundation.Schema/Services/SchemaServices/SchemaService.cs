using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Foundation.Schema.Models;
using Foundation.Schema.ResultModels;
using Foundation.Schema.Validators;
using Foundation.TenantDatabase.Areas.Taxonomies.Entities;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Helpers;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Schema.Services.SchemaServices
{
    public class SchemaService : ISchemaService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly IEntityService entityService;

        public SchemaService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            IEntityService entityService
            )
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.entityService = entityService;
        }

        public async Task<SchemaResultModel> CreateAsync(CreateSchemaModel model)
        {
            var validator = new CreateSchemaModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createSchemaResultModel = new SchemaResultModel();

            if (!validationResult.IsValid)
            {
                createSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return createSchemaResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                createSchemaResultModel.Errors.Add(ErrorConstants.NoTenant);
                return createSchemaResultModel;
            }

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Translation>(tenant.DatabaseConnectionString);

            foreach (var property in model.JsonSchema["properties"].OfType<JProperty>())
            {
                foreach (var propertyValue in property)
                {
                    var titleTranslation = new Translation
                    {
                        Key = $"Product Schema | {propertyValue.Value<string>("title").Replace(":", string.Empty)}",
                        Value = propertyValue.Value<string>("title"),
                        Language = model.Language
                    };

                    await translationRepository.CreateAsync(this.entityService.EnrichEntity(titleTranslation, model.Username));

                    await translationRepository.SaveChangesAsync();

                    propertyValue["title"] = titleTranslation.Id;
                }
            }

            var schema = new TenantDatabase.Areas.Schemas.Entities.Schema
            {
                Name = model.Name,
                EntityTypeId = model.EntityTypeId,
                JsonSchema = model.JsonSchema?.ToString(),
                UiSchema = model.UiSchema?.ToString()
            };

            var schemaRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<TenantDatabase.Areas.Schemas.Entities.Schema>(tenant.DatabaseConnectionString);

            await schemaRepository.CreateAsync(this.entityService.EnrichEntity(schema, model.Username));

            await schemaRepository.SaveChangesAsync();

            var translation = new Translation
            {
                Key = schema.Id.ToString(),
                Value = schema.Name,
                Language = model.Language
            };

            await translationRepository.CreateAsync(this.entityService.EnrichEntity(translation, model.Username));

            await translationRepository.SaveChangesAsync();

            createSchemaResultModel.Schema = schema;

            return createSchemaResultModel;
        }


        public async Task<SchemaResultModel> GetByIdAsync(GetSchemaModel getSchemaModel)
        {
            var validator = new GetSchemaModelValidator();

            var validationResult = await validator.ValidateAsync(getSchemaModel);

            var getSchemaResultModel = new SchemaResultModel();

            if (!validationResult.IsValid)
            {
                getSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return getSchemaResultModel;
            }

            var tenant = this.tenantRepository.GetById(getSchemaModel.TenantId.Value);

            if (tenant == null)
            {
                getSchemaResultModel.Errors.Add(ErrorConstants.NoTenant);
                return getSchemaResultModel;
            }

            var schemaRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Schemas.Entities.Schema>(tenant.DatabaseConnectionString);

            var schema = schemaRepository.GetById(getSchemaModel.Id.Value);

            if (schema == null)
            {
                getSchemaResultModel.Errors.Add(ErrorConstants.NotFound);
                return getSchemaResultModel;
            }

            var translationRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Translations.Entities.Translation>(tenant.DatabaseConnectionString);

            var taxonomyRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Taxonomies.Entities.Taxonomy>(tenant.DatabaseConnectionString); 

            var jsonSchema = JObject.Parse(schema.JsonSchema);

            var properties = (JObject)jsonSchema["properties"];

            foreach (var property in properties)
            {
                var propertyDetails = (JObject)property.Value;

                var propertyTitle = (string)propertyDetails["title"];

                Guid propertyTitleGuid;

                var isPropertyTitleGuid = Guid.TryParse(propertyTitle, out propertyTitleGuid);

                if (!string.IsNullOrWhiteSpace(propertyTitle) && isPropertyTitleGuid)
                {
                    propertyDetails["title"] = TranslationHelper.Text(translationRepository, propertyTitleGuid, getSchemaModel.Language);
                }
            }

            var definitons = (JObject)jsonSchema["definitions"];

            if (definitons != null && definitons.Children().Any())
            {
                foreach (var definition in definitons)
                {
                    Guid definitionKeyId;

                    var isDefinitionKeyGuid = Guid.TryParse(definition.Key, out definitionKeyId);

                    if (isDefinitionKeyGuid)
                    {
                        var taxonomy = taxonomyRepository.Get(x => x.Id == definitionKeyId && x.IsActive).FirstOrDefault();

                        if (taxonomy != null)
                        {
                            var definitionValue = (JObject)definition.Value;

                            definitionValue.Add("type", "string");
                            definitionValue.Add("title", taxonomy.Name);

                            var flattenedTaxonomies = this.GetFlatTaxonomyDescendants(tenant.DatabaseConnectionString, taxonomy.Id);

                            var definitionItems = new JArray();

                            foreach (var flattenedTaxonomy in flattenedTaxonomies)
                            {
                                var flattenedTaxonomyTitle = TranslationHelper.Text(translationRepository, flattenedTaxonomy.Id.ToString(), getSchemaModel.Language);

                                definitionItems.Add(new JObject(
                                        new JProperty("type", "string"),
                                        new JProperty("title", flattenedTaxonomyTitle),
                                        new JProperty("enum", new JArray(flattenedTaxonomy.Id))
                                    ));
                            }

                            definitionValue.Add("anyOf", definitionItems);
                        }
                    }
                }
            }

            getSchemaResultModel.Schema = new TenantDatabase.Areas.Schemas.Entities.Schema
            { 
                Id = schema.Id,
                Name = schema.Name,
                JsonSchema = jsonSchema.ToString(),
                UiSchema = schema.UiSchema,
                EntityTypeId = schema.EntityTypeId,
                LastModifiedDate = schema.LastModifiedDate,
                LastModifiedBy = schema.LastModifiedBy,
                CreatedDate = schema.CreatedDate,
                CreatedBy = schema.CreatedBy
            };

            return getSchemaResultModel;
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
                            ParentId = (Guid)reader["ParentId"],
                            Name = (string)reader["Name"],
                            Order = (int)reader["Order"],
                            IsActive = (bool)reader["IsActive"],
                            LastModifiedDate = (DateTime)reader["LastModifiedDate"],
                            LastModifiedBy = (string)reader["LastModifiedBy"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            CreatedBy = (string)reader["CreatedBy"]
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