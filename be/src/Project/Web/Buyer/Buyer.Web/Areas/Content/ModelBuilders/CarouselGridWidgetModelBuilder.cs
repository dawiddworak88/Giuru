using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.ViewModel;
using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Shared.Repositories.Products;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.ModelBuilders
{
    public class CarouselGridWidgetModelBuilder : IAsyncComponentModelBuilder<CarouselGridWidgetComponentModel, CarouselGridWidgetViewModel>
    {
        private readonly ICatalogProductsRepository _catalogProductsRepository;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;

        public CarouselGridWidgetModelBuilder(
            ICatalogProductsRepository catalogProductsRepository,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            _catalogProductsRepository = catalogProductsRepository;
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
        }

        public async Task<CarouselGridWidgetViewModel> BuildModelAsync(CarouselGridWidgetComponentModel componentModel)
        {
            var viewModel = new CarouselGridWidgetViewModel
            {
                Title = componentModel.Title,

            };

            var products = await _catalogProductsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, componentModel.Skus);

            var items = new List<CarouselGridItemWidgetViewModel>();

            if (products is not null && products.OrEmptyIfNull().Any())
            {

                var contentGridCarouselItems = new List<CarouselGridCarouselItemWidgetViewModel>();

                foreach (var product in products)
                {
                    var carouselItem = new CarouselGridCarouselItemWidgetViewModel
                    {
                        Title = product.Name,
                        Subtitle = product.Sku,
                        Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(product.Images.FirstOrDefault(), 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(product.Images.FirstOrDefault(), 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(product.Images.FirstOrDefault(), 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(product.Images.FirstOrDefault(), 768) },
                        }
                    };

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new CarouselGridItemWidgetViewModel { Title = componentModel.Title, CarouselItems = contentGridCarouselItems });
            }

            viewModel.Items = items;

            return viewModel;
        }
    }
}
