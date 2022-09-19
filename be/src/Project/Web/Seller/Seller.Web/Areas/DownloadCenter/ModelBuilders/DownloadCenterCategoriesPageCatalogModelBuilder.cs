using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenterCategories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoriesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterCategory>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository;

        public DownloadCenterCategoriesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            LinkGenerator linkGenerator,
            IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.downloadCenterCategoriesRepository = downloadCenterCategoriesRepository;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
        }

        public async Task<CatalogViewModel<DownloadCenterCategory>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<DownloadCenterCategory>, DownloadCenterCategory>();

            viewModel.Title = this.globalLocalizer.GetString("DownloadCenterCategories");
            viewModel.NewText = this.downloadCenterLocalizer.GetString("NewCategory");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "DownloadCenterCategory", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "DownloadCenterCategory", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "DownloadCenterCategoriesApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "DownloadCenterCategoriesApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(DownloadCenterCategory.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("Name"),
                    this.globalLocalizer.GetString("ParentCategory"),
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
                        Title = nameof(DownloadCenterCategory.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterCategory.ParentCategoryName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterCategory.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterCategory.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.downloadCenterCategoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(DownloadCenterCategory.CreatedDate)} desc");

            return viewModel;
        }
    }
}
