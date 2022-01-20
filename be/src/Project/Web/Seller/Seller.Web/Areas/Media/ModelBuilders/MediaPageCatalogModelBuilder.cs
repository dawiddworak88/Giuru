using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer mediaLocalizer;
        private readonly LinkGenerator linkGenerator;

        public MediaPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<MediaResources> mediaLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.mediaLocalizer = mediaLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<MediaItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<MediaItem>, MediaItem>();

            viewModel.Title = this.mediaLocalizer.GetString("Media");
            viewModel.NewText = this.mediaLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(MediaItem.CreatedDate)} desc";

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
                        Title = nameof(MediaItem.Id).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(MediaItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            return viewModel;
        }
    }
}