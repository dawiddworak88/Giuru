using Foundation.Catalog.Infrastructure;
using Foundation.Extensions.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.CategorySchemas
{
    public class CategorySchemaService : ICategorySchemaService
    {
        private readonly CatalogContext context;

        public CategorySchemaService(CatalogContext context)
        {
            this.context = context;
        }

        public async Task RebuildCategorySchemasAsync()
        {
            var categorySchemasIds = this.context.CategorySchemas.Where(x => x.IsActive).Select(x => x.Id).ToList();

            foreach (var categorySchemaId in categorySchemasIds)
            {
                var categorySchema = this.context.CategorySchemas.FirstOrDefault(x => x.Id == categorySchemaId);

                if (!string.IsNullOrWhiteSpace(categorySchema.Schema))
                {
                    var schemaObject = JObject.Parse(categorySchema.Schema);

                    var definitionsObject = (JObject)schemaObject["definitions"];

                    if (definitionsObject != null)
                    {
                        var newDefinitions = new JObject();

                        foreach (JProperty definitionProperty in definitionsObject.Children().OrEmptyIfNull())
                        {
                            var productAttribute = this.context.ProductAttributes.FirstOrDefault(x => x.Id == Guid.Parse(definitionProperty.Name) && x.IsActive);

                            if (productAttribute != null)
                            {
                                var newProductAttributeObject = new JObject();

                                var productAttributeTranslation = productAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.Language == categorySchema.Language && x.IsActive);

                                if (productAttributeTranslation == null)
                                {
                                    productAttributeTranslation = productAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.IsActive);
                                }

                                newProductAttributeObject.Add(new JProperty("title", new JValue(productAttributeTranslation.Name)));

                                newProductAttributeObject.Add(new JProperty("type" , new JValue("string")));

                                var productAttributeItems = this.context.ProductAttributeItems.Where(x => x.ProductAttributeId == productAttribute.Id && x.IsActive).ToList();

                                var newAnyOfArray = new JArray();

                                foreach (var productAttributeItem in productAttributeItems.OrEmptyIfNull())
                                {
                                    var newProductAttributeItemObject = new JObject();
                                    newProductAttributeItemObject.Add(new JProperty("type", new JValue("string")));
                                    newProductAttributeItemObject.Add(new JProperty("enum", new JArray(new JValue(productAttributeItem.Id))));

                                    var productAttributeItemTranslation = productAttributeItem.ProductAttributeItemTranslations.FirstOrDefault(x => x.Language == categorySchema.Language && x.IsActive);

                                    if (productAttributeItemTranslation == null)
                                    {
                                        productAttributeItemTranslation = productAttributeItem.ProductAttributeItemTranslations.FirstOrDefault(x => x.IsActive);
                                    }

                                    newProductAttributeItemObject.Add(new JProperty("title", new JValue(productAttributeItemTranslation.Name)));

                                    newAnyOfArray.Add(newProductAttributeItemObject);
                                }                                

                                newProductAttributeObject.Add(new JProperty("anyOf", newAnyOfArray));

                                newDefinitions.Add(new JProperty(productAttribute.Id.ToString(), newProductAttributeObject));
                            }
                        }

                        schemaObject["definitions"] = newDefinitions;
                    }

                    categorySchema.Schema = schemaObject.ToString();
                }

                await this.context.SaveChangesAsync();
            }
        }
    }
}
