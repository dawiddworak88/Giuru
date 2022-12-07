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
        private readonly IHeroSliderRepository heroSliderRepository;
        private readonly IOptionsMonitor<AppSettings> options;
        private readonly IMediaService mediaService;

        public HeroSliderModelBuilder(
            IHeroSliderRepository heroSliderRepository,
            IOptionsMonitor<AppSettings> options,
            IMediaService mediaService)
        {
            this.heroSliderRepository = heroSliderRepository;
            this.options = options;
            this.mediaService = mediaService;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var heroSliderItemViewModels = new List<HeroSliderItemViewModel>();

            var heroSliderItems = await heroSliderRepository.GetHeroSliderItemsAsync(componentModel.Language, this.options.CurrentValue.DefaultCulture);

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
                            ImageSrc = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc),
                            ImageTitle = heroSliderItem.Image?.ImageTitle,
                            Sources = new List<SourceViewModel>
                            {
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1920) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1600) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1024) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 414) },
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1600) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 1024) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(heroSliderItem.Image?.ImageSrc, 414) }
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
