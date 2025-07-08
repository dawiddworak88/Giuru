using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Products.Repositories;
using Foundation.GenericRepository.Definitions;
using Seller.Web.Shared.DomainModels.Products;

namespace Seller.Web.Areas.ProductAttributeItems.ModelBuilders
{
    public class ProductAttributePageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttributeItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IProductAttributeItemsRepository productAttributeItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributePageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IProductAttributesRepository productAttributesRepository,
            IProductAttributeItemsRepository productAttributeItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productAttributesRepository = productAttributesRepository;
            this.productAttributeItemsRepository = productAttributeItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<ProductAttributeItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<ProductAttributeItem>, ProductAttributeItem>();

            viewModel.NewText = this.productLocalizer.GetString("NewProductAttributeItem");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewUrl = this.linkGenerator.GetPathByAction("New", "ProductAttributeItem", new { Id = componentModel.Id, Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttributeItem", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "ProductAttributeItemsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductAttributeItemsApi", new { ProductAttributeId = componentModel.Id, Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ProductAttributeItem.CreatedDate)} desc";

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
                        Title = nameof(ProductAttributeItem.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ProductAttributeItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ProductAttributeItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.productAttributeItemsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(ProductAttributeItem.CreatedDate)} desc");

            return viewModel;
        }
    }
}