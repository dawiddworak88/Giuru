using Foundation.Extensions.Definitions;
using Foundation.GenericRepository.Services;
using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using Catalog.Api.v1.Areas.Schemas.Validators;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Catalog.Api.Infrastructure.Translations.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure.Schemas.Entities;
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
            //var validator = new CreateSchemaModelValidator();

            //var validationResult = await validator.ValidateAsync(model);

            //var createSchemaResultModel = new SchemaResultModel();

            //if (!validationResult.IsValid)
            //{
            //    createSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            //    return createSchemaResultModel;
            //}

            //var seller = this.sellerRepository.GetById(model.SellerId.Value);

            //if (seller == null)
            //{
            //    createSchemaResultModel.Errors.Add(ErrorConstants.NoSeller);
            //    return createSchemaResultModel;
            //}

            //foreach (var property in model.JsonSchema["properties"].OfType<JProperty>())
            //{
            //    foreach (var propertyValue in property)
            //    {
            //        var titleTranslation = new Translation
            //        {
            //            Key = $"Product Schema | {propertyValue.Value<string>("title").Replace(":", string.Empty)}",
            //            Value = propertyValue.Value<string>("title"),
            //            Language = model.Language
            //        };

            //        await this.context.Translations.AddAsync(this.entityService.EnrichEntity(titleTranslation, model.Username));

            //        await this.context.SaveChangesAsync();

            //        propertyValue["title"] = titleTranslation.Id;
            //    }
            //}

            //var schema = new Schema
            //{
            //    JsonSchema = model.JsonSchema?.ToString(),
            //    UiSchema = model.UiSchema?.ToString()
            //};

            //await this.context.Schemas.AddAsync(this.entityService.EnrichEntity(schema, model.Username));

            //await this.context.SaveChangesAsync();

            //var translation = new Translation
            //{
            //    Key = schema.Id.ToString(),
            //    Language = model.Language
            //};

            //await this.context.Translations.AddAsync(this.entityService.EnrichEntity(translation, model.Username));

            //await this.context.SaveChangesAsync();

            //createSchemaResultModel.Schema = schema;

            //return createSchemaResultModel;

            return default;
        }


        public async Task<SchemaResultModel> GetByIdAsync(GetSchemaModel getSchemaModel)
        {
            //var validator = new GetSchemaModelValidator();

            //var validationResult = await validator.ValidateAsync(getSchemaModel);

            //var getSchemaResultModel = new SchemaResultModel();

            //if (!validationResult.IsValid)
            //{
            //    getSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            //    return getSchemaResultModel;
            //}

            //var seller = this.sellerRepository.GetById(getSchemaModel.SellerId.Value);

            //if (seller == null)
            //{
            //    getSchemaResultModel.Errors.Add(ErrorConstants.NoSeller);
            //    return getSchemaResultModel;
            //}

            //var schema = this.context.Schemas.FirstOrDefault(x => x.Id == getSchemaModel.Id.Value && x.IsActive);

            //if (schema == null)
            //{
            //    getSchemaResultModel.Errors.Add(ErrorConstants.NotFound);
            //    return getSchemaResultModel;
            //}

            //getSchemaResultModel.Schema = new Schema
            //{ 
            //    Id = schema.Id,
            //    JsonSchema = await this.GetJsonSchemaAsync(schema.JsonSchema, getSchemaModel.Language),
            //    UiSchema = schema.UiSchema,
            //    LastModifiedDate = schema.LastModifiedDate,
            //    CreatedDate = schema.CreatedDate
            //};

            //return getSchemaResultModel;

            return default;
        }

        public async Task<SchemaResultModel> GetByEntityTypeIdAsync(GetSchemaByEntityTypeModel getSchemaModel)
        {
            //var validator = new GetSchemaByEntityTypeModelValidator();

            //var validationResult = await validator.ValidateAsync(getSchemaModel);

            //var getSchemaResultModel = new SchemaResultModel();

            //if (!validationResult.IsValid)
            //{
            //    getSchemaResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            //    return getSchemaResultModel;
            //}

            //var seller = this.sellerRepository.GetById(getSchemaModel.SellerId.Value);

            //if (seller == null)
            //{
            //    getSchemaResultModel.Errors.Add(ErrorConstants.NoSeller);
            //    return getSchemaResultModel;
            //}

            //var schema = this.context.Schemas.FirstOrDefault(x => x.Id == getSchemaModel.Id && x.IsActive);

            //if (schema == null)
            //{
            //    getSchemaResultModel.Errors.Add(ErrorConstants.NotFound);
            //    return getSchemaResultModel;
            //}

            //getSchemaResultModel.Schema = new Schema
            //{
            //    Id = schema.Id,
            //    JsonSchema = await this.GetJsonSchemaAsync(schema.JsonSchema, getSchemaModel.Language),
            //    UiSchema = schema.UiSchema,
            //    LastModifiedDate = schema.LastModifiedDate,
            //    CreatedDate = schema.CreatedDate
            //};

            //return getSchemaResultModel;

            return default;
        }

        private async Task<string> GetJsonSchemaAsync(string jsonSchemaSerialized, string language, string connectionString = null)
        {
            var jsonSchema = JObject.Parse(jsonSchemaSerialized);

            var properties = (JObject)jsonSchema["properties"];

            var translations = this.context.Translations.Where(x => x.IsActive).ToList();

            foreach (var property in properties)
            {
                var propertyDetails = (JObject)property.Value;

                var propertyTitle = (string)propertyDetails["title"];

                Guid propertyTitleGuid;

                var isPropertyTitleGuid = Guid.TryParse(propertyTitle, out propertyTitleGuid);

                if (!string.IsNullOrWhiteSpace(propertyTitle) && isPropertyTitleGuid)
                {
                    // propertyDetails["title"] = TranslationHelper.Text(translations, propertyTitleGuid, language);
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
                        var taxonomy = this.context.Taxonomies.FirstOrDefault(x => x.Id == definitionKeyId && x.IsActive);

                        if (taxonomy != null)
                        {
                            var definitionValue = (JObject)definition.Value;

                            definitionValue.Add("type", "string");
                            definitionValue.Add("title", taxonomy.Name);

                            var flattenedTaxonomies = this.GetFlatTaxonomyDescendants(connectionString, taxonomy.Id);

                            var definitionItems = new JArray();

                            foreach (var flattenedTaxonomy in flattenedTaxonomies)
                            {
                                //var flattenedTaxonomyTitle = TranslationHelper.Text(translations, flattenedTaxonomy.Id.ToString(), language);

                                //definitionItems.Add(new JObject(
                                //        new JProperty("type", "string"),
                                //        new JProperty("title", flattenedTaxonomyTitle),
                                //        new JProperty("enum", new JArray(flattenedTaxonomy.Id))
                                //    ));
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
                            ParentId = (Guid)reader["ParentId"],
                            Name = (string)reader["Name"],
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