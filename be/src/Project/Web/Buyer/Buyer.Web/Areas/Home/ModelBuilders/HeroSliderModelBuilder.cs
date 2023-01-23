using Buyer.Web.Areas.Home.Repositories;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        private readonly IHeroSliderRepository _heroSliderRepository;
        private readonly IOptionsMonitor<AppSettings> _options;
        private readonly IMediaService _mediaService;

        public HeroSliderModelBuilder(
            IHeroSliderRepository heroSliderRepository,
            IOptionsMonitor<AppSettings> options,
            IMediaService mediaService)
        {
            _heroSliderRepository = heroSliderRepository;
            _options = options;
            _mediaService = mediaService;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var heroSliderItemViewModels = new List<HeroSliderItemViewModel>();

            var heroSliderItems = await _heroSliderRepository.GetHeroSliderItemsAsync(componentModel.Language, _options.CurrentValue.DefaultCulture);

            if (heroSliderItems is not null)
            {
                foreach (var heroSliderItem in heroSliderItems)
                {
                    heroSliderItemViewModels.Add(new HeroSliderItemViewModel
                    {
                        TeaserTitle = heroSliderItem.TeaserTitle,
                        TeaserText = heroSliderItem.TeaserText,
                        CtaText = heroSliderItem.CtaText,
                        CtaUrl = heroSliderItem.CtaUrl,
                        Image = new ImageViewModel
                        {
                            ImageAlt = heroSliderItem.Image?.ImageAlt,
                            ImageSrc = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc),
                            ImageTitle = heroSliderItem.Image?.ImageTitle,
                            Sources = new List<SourceViewModel>
                            {
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1920) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1600) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1024) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 414) },
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1600) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1024) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 414) }
                            }
                        }
                    });
                }
            }

            return new HeroSliderViewModel
            { 
                Items = heroSliderItemViewModels
            };
        }
    }
}
