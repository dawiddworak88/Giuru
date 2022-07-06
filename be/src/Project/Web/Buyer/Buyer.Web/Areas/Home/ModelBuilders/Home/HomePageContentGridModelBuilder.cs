using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel.Home;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.Catalogs;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
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

namespace Buyer.Web.Areas.Home.ModelBuilders.Home
{
    public class HomePageContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HomePageContentGridModelBuilder(
            ICatalogService catalogService,
            IOptions<AppSettings> options,
            IMediaService mediaService,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.catalogService = catalogService;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<HomePageContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var categories = await this.catalogService.GetCatalogCategoriesAsync(
                componentModel.Language, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, "Order");

            foreach (var category in categories.OrEmptyIfNull().Where(x => x.Level == HomeConstants.Categories.FirstLevel))
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
                    contentItem.ImageUrl = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, ProductConstants.ProductsCatalogItemImageWidth);
                    contentItem.Sources = new List<SourceViewModel>
                    {
                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 1366) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 470) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 342) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 768) },

                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 1366) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 470) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 342) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 768) }
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
