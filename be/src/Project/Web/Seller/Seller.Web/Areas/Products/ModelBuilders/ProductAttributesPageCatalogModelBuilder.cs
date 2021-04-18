using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Products.DomainModels;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductAttributesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttribute>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productAttributesRepository = productAttributesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<ProductAttribute>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<ProductAttribute>, ProductAttribute>();

            viewModel.Title = this.globalLocalizer.GetString("ProductAttributes");

            viewModel.NewText = this.productLocalizer.GetString("NewProductAttribute");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "ProductAttributesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductAttributesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ProductAttribute.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("Name"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ProductAttribute.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ProductAttribute.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ProductAttribute.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.productAttributesRepository.GetAsync(componentModel.Token, componentModel.Language, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage, $"{nameof(ProductAttribute.CreatedDate)} desc");

            return viewModel;
        }
    }
}