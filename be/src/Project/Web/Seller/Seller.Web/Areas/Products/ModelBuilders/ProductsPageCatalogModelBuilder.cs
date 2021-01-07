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

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Product>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Product>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<Product>, Product>();

            viewModel.Title = this.productLocalizer.GetString("Products");

            viewModel.NewText = this.productLocalizer.GetString("NewProduct");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            
            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            
            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[] 
                { 
                    this.globalLocalizer.GetString("Sku"),
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
                        Title = nameof(Product.Sku).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Product.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Product.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Product.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.productsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, null, false, componentModel.SellerId, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage, $"{nameof(Product.CreatedDate)} desc");

            return viewModel;
        }
    }
}