using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Areas.Shared.Definitions.Products;
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel>
    {
        private readonly ICatalogService _catalogService;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public HomePageContentGridModelBuilder(
            ICatalogService catalogService,
            IMediaService mediaService,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _catalogService = catalogService;
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
        }

        public async Task<HomePageContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var categories = await _catalogService.GetCatalogCategoriesAsync(
                componentModel.Language, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, "Order");

            foreach (var category in categories.OrEmptyIfNull().Where(x => x.Level == HomeConstants.Categories.FirstLevel))
            {
                var contentItem = new ContentGridItemViewModel
                {
                    Id = category.Id,
                    Title = category.Name,
                    ImageAlt = category.Name,
                    Url = _linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, category.Id })
                };

                if (category.ThumbnailMediaId.HasValue)
                {
                    contentItem.ImageUrl = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, ProductConstants.ProductsCatalogItemImageWidth);
                    contentItem.Sources = new List<SourceViewModel>
                    {
                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 1366) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 470) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 342) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 768) },

                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 1366) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 470) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 342) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(category.ThumbnailMediaId.Value, 768) }
                    };
                }

                items.Add(contentItem);
            }

            var viewModel = new HomePageContentGridViewModel
            {
                Title = _globalLocalizer.GetString("Categories"),
                Items = items
            };

            return viewModel;
        }
    }
}
