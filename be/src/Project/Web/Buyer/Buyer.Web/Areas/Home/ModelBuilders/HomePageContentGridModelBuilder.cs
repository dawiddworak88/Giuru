using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.Catalogs;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly ICdnService cdnService;

        public HomePageContentGridModelBuilder(
            ICatalogService catalogService,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICdnService cdnService)
        {
            this.catalogService = catalogService;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.cdnService = cdnService;
        }

        public async Task<HomePageContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var categories = await this.catalogService.GetCatalogCategoriesAsync(
                componentModel.Language, 
                Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, 
                Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage,
                "Order");

            foreach (var category in categories.OrEmptyIfNull().Where(x => x.Level == CategoriesConstants.FirstLevel))
            {
                var contentItem = new ContentGridItemViewModel
                {
                    Id = category.Id,
                    Title = category.Name,
                    ImageAlt = category.Name,
                    Url = this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, category.Id })
                };

                if (category.ThumbnailMediaId.HasValue)
                {
                    contentItem.ImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, ProductConstants.ProductsCatalogItemImageWidth, ProductConstants.ProductsCatalogItemImageHeight, true));
                    contentItem.Sources = new List<SourceViewModel>
                    {
                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 1366, 1366, true, MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 470, 470, true,MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 342, 342, true, MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 768, 768, true, MediaConstants.WebpExtension)) },

                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 1366, 1366, true)) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 470, 470, true)) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 342, 342, true)) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, category.ThumbnailMediaId.Value, 768, 768, true)) }
                    };
                }

                items.Add(contentItem);
            }

            var viewModel = new HomePageContentGridViewModel
            {
                Title = this.globalLocalizer.GetString("Categories"),
                Items = items
            };

            return viewModel;
        }
    }
}
