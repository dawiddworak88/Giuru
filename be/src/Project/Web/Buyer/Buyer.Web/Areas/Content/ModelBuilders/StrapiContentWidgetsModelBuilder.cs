using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.DomainModel;
using Buyer.Web.Areas.Content.Repositories;
using Buyer.Web.Areas.Content.ViewModel;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.ModelBuilders
{
    public class StrapiContentWidgetsModelBuilder : IAsyncComponentModelBuilder<ContentComponentModel, StrapiContentWidgetsViewModel>
    {
        private readonly IAsyncComponentModelBuilder<CarouselGridComponentModel, CarouselGridViewModel> _carouselWidgetModelBuilder;
        private readonly IContentRepository _slugRepository;

        public StrapiContentWidgetsModelBuilder(
            IAsyncComponentModelBuilder<CarouselGridComponentModel, CarouselGridViewModel> carouselWidgetModelBuilder,
            IContentRepository slugRepository)
        {
            _carouselWidgetModelBuilder = carouselWidgetModelBuilder;
            _slugRepository = slugRepository;
        }

        public async Task<StrapiContentWidgetsViewModel> BuildModelAsync(ContentComponentModel componentModel)
        {
            var contentPage = await _slugRepository.GetContentPageBySlugAsync(componentModel.Language, "en", componentModel.Slug);

            if (contentPage is not null)
            {
                var viewModel = new StrapiContentWidgetsViewModel
                {
                    Title = contentPage.Title,
                };

                if (contentPage.ReturnButton is not null)
                {
                    viewModel.ReturnButton = new StrapiReturnButtonViewModel
                    {
                        Href = contentPage.ReturnButton.Href,
                        Label = contentPage.ReturnButton.Label
                    };
                }

                var widgets = new List<StrapiWidgetViewModel>();

                foreach (var sharedComponent in contentPage.SharedComponents.OrEmptyIfNull())
                {
                    if (sharedComponent is SharedSliderComponent sharedSlider)
                    {
                        var sliderComponent = new StrapiSliderWidgetViewModel
                        {
                            Typename = sharedSlider.Typename,
                            Slider = await _carouselWidgetModelBuilder.BuildModelAsync(new CarouselGridComponentModel
                            {
                                Title = sharedSlider.Title,
                                Skus = sharedSlider.Skus,
                                IsAuthenticated = componentModel.IsAuthenticated,
                                BasketId = componentModel.BasketId,
                                Language = componentModel.Language,
                                Name = componentModel.Name,
                                Token = componentModel.Token,
                                SellerId = componentModel.SellerId
                            })
                        };

                        widgets.Add(sliderComponent);
                    }
                    else if (sharedComponent is SharedContentComponent sharedContent)
                    {
                        var contentComponent = new StrapiContentWidgetViewModel
                        {
                            Typename = sharedContent.Typename,
                            Content = sharedContent.Content
                        };

                        widgets.Add(contentComponent);
                    }
                    else if (sharedComponent is BlocksVideoComponent blocksVideo)
                    {
                        var videoComponent = new StrapiVideoWidgetViewModel
                        {
                            Typename = blocksVideo.Typename,
                            Type = blocksVideo.Type,
                            VideoUrl = blocksVideo.VideoUrl,
                            VideoType = blocksVideo.VideoType
                        };

                        widgets.Add(videoComponent);
                    }
                }

                viewModel.Widgets = widgets;

                return viewModel;
            }

            return default;
        }
    }
}
