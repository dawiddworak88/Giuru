using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.downloadCenterRepository = downloadCenterRepository;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
        }

        public async Task<CatalogViewModel<DownloadCenterItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<DownloadCenterItem>, DownloadCenterItem>();

            viewModel.Title = this.globalLocalizer.GetString("DownloadCenter");
            viewModel.NewText = this.downloadCenterLocalizer.GetString("NewDownloadCenter");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "DownloadCenterItem", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "DownloadCenterItem", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(DownloadCenterItem.CreatedDate)} desc";
            viewModel.ConfirmationDialogDeleteNameProperty = new List<string>
            {
                nameof(DownloadCenterItem.CategoryName).ToCamelCase(),
            };

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("Category"),
                    this.globalLocalizer.GetString("Order"),
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
                        Title = nameof(DownloadCenterItem.CategoryName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterItem.Order).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(DownloadCenterItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.downloadCenterRepository.GetDownloadCenterAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(DownloadCenterItem.CreatedDate)} desc");

            return viewModel;
        }
    }
}
