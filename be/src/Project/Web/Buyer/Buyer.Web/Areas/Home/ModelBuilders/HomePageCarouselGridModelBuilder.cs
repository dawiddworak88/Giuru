using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Services.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Foundation.Media.Services.MediaServices;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageCarouselGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public HomePageCarouselGridModelBuilder(
            ICatalogService catalogService,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaService)
        {
            this.catalogService = catalogService;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaService;
        }

        public async Task<HomePageCarouselGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<CarouselGridItemViewModel>();

            var newProducts = await this.catalogService.GetCatalogProductsAsync(
                componentModel.Token,
                componentModel.Language,
                null,
                false,
                true,
                null,
                Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex,
                Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);

            if (newProducts != null && newProducts.Total > 0)
            {
                var contentGridCarouselItems = new List<CarouselGridCarouselItemViewModel>();

                foreach (var newProduct in newProducts.Data)
                {
                    var carouselItem = new CarouselGridCarouselItemViewModel
                    {
                        Id = newProduct.Id,
                        Title = newProduct.Title,
                        Subtitle = newProduct.Sku,
                        ImageAlt = newProduct.ImageAlt,
                        ImageUrl = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 1920),
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, newProduct.Id }),
                        Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 768) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(newProduct.ImageUrl, 768) }
                        }
                    };

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new CarouselGridItemViewModel { Id = HomeConstants.Novelties.NoveltiesId, Title = this.globalLocalizer.GetString("Novelties"), CarouselItems = contentGridCarouselItems });
            }

            var viewModel = new HomePageCarouselGridViewModel
            {
                Items = items
            };

            return viewModel;
        }
    }
}
