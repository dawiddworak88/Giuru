using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Areas.News.Repositories.Categories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class CategoriesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer newsLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator,
            ICategoriesRepository categoriesRepository)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
            this.linkGenerator = linkGenerator;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<CatalogViewModel<Category>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<Category>, Category>();

            viewModel.Title = this.newsLocalizer.GetString("Categories");
            viewModel.NewText = this.newsLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Category", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Category", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "CategoriesApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "CategoriesApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Category.CreatedDate)} desc";

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
                        Title = nameof(Category.Name).ToCamelCase(),
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

            viewModel.PagedItems = await this.categoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Category.CreatedDate)} desc");

            return viewModel;
        }
    }
}
