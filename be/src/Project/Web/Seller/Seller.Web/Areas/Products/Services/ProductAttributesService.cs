using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Services
{
    public class ProductAttributesService : IProductAttributesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductAttributesRepository _productAttributesRepository;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public ProductAttributesService(
            ICategoriesRepository categoriesRepository,
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _categoriesRepository = categoriesRepository;
            _productAttributesRepository = productAttributesRepository;
            _productLocalizer = productLocalizer;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var categoriesSchema = await _categoriesRepository.GetAllSchemasAsync(token, language);

            if (IsAttributeImplementInCategorySchema(id, categoriesSchema))
            {
                throw new CustomException(_productLocalizer.GetString("ProductAttributeIsInUse"), (int)HttpStatusCode.Conflict);
            }
            else
            {
                await _productAttributesRepository.DeleteAsync(
                    token,
                    language,
                    id);
            }
        }

        private bool IsAttributeImplementInCategorySchema(Guid? id, IEnumerable<CategorySchemas> categoriesSchema)
        {
            var attributeId = id.ToString();

            foreach (var categorySchemas in categoriesSchema.OrEmptyIfNull())
            {
                foreach (var categorySchema in categorySchemas.Schemas.OrEmptyIfNull())
                {
                    if (categorySchema.Schema.Contains(attributeId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
