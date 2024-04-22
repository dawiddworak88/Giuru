using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.ViewModel;
using Buyer.Web.Shared.Repositories.Products;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.ModelBuilders
{
    public class CarouselGridModelBuilder : IAsyncComponentModelBuilder<CarouselGridComponentModel, CarouselGridViewModel>
    {
        private readonly ICatalogProductsRepository _catalogProductsRepository;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;

        public CarouselGridModelBuilder(
            ICatalogProductsRepository catalogProductsRepository,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            _catalogProductsRepository = catalogProductsRepository;
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
        }

        public async Task<CarouselGridViewModel> BuildModelAsync(CarouselGridComponentModel componentModel)
        {
            var viewModel = new CarouselGridViewModel
            {
                Title = componentModel.Title,

            };

            var products = await _catalogProductsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, componentModel.Skus);

            var contentGridItems = new List<CarouselGridItemViewModel>();

            if (products is not null && products.OrEmptyIfNull().Any())
            {
                var contentGridCarouselItems = new List<CarouselGridCarouselItemViewModel>();

                foreach (var product in products)
                {
                    var carouselItem = new CarouselGridCarouselItemViewModel
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

                contentGridItems.Add(new CarouselGridItemViewModel { Title = componentModel.Title, CarouselItems = contentGridCarouselItems });
            }

            viewModel.Items = contentGridItems;

            return viewModel;
        }
    }
}
