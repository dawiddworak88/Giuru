using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductCardsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductCardCategory>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public ProductCardsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _categoriesRepository = categoriesRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<ProductCardCategory>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<ProductCardCategory>, ProductCardCategory>();

            viewModel.Title = _globalLocalizer.GetString("ProductCards");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;
            viewModel.NewText = _productLocalizer.GetString("NewProductCard");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "ProductCard", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "CategoriesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "CategoriesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ProductCardCategory.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("ParentCategory"),
                    _globalLocalizer.GetString("LastModifiedDate"),
                    _globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Category.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Category.ParentCategoryName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Category.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Category.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            var categories = await _categoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Category.CreatedDate)} desc");

            if (categories.Data.OrEmptyIfNull().Any())
            {
                viewModel.PagedItems = new PagedResults<IEnumerable<ProductCardCategory>>(categories.Total, categories.PageSize)
                {
                    Data = categories.Data.Select(x => new ProductCardCategory 
                    { 
                        Id = x.Id,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        ParentCategoryName = x.ParentCategoryName,
                        Level = x.Level,
                        Order = x.Order,
                        IsLeaf = x.IsLeaf,
                        ThumbnailMediaId = x.ThumbnailMediaId,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };
            }

            return viewModel;
        }
    }
}
