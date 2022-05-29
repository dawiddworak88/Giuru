using Buyer.Web.Areas.Home.Repositories;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.Images;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        private readonly IHeroSliderRepository heroSliderRepository;
        private readonly IStringLocalizer<HeroSliderResources> heroSliderLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly ICdnService cdnService;

        public HeroSliderModelBuilder(
            IHeroSliderRepository heroSliderRepository,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IStringLocalizer<HeroSliderResources> heroSliderLocalizer,
            LinkGenerator linkGenerator,
            ICdnService cdnService)
        {
            this.heroSliderRepository = heroSliderRepository;
            this.options = options;
            this.mediaService = mediaService;
            this.heroSliderLocalizer = heroSliderLocalizer;
            this.linkGenerator = linkGenerator;
            this.cdnService = cdnService;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var heroSliderItemViewModels = new List<HeroSliderItemViewModel>();

            var heroSliderItems = await this.heroSliderRepository.GetHeroSliderItemsAsync(componentModel.Language, this.options.Value.DefaultCulture);

            if (heroSliderItems is not null)
            {
                foreach (var heroSliderItem in heroSliderItems.OrEmptyIfNull())
                {
                    heroSliderItemViewModels.Add(new HeroSliderItemViewModel
                    {
                        TeaserTitle = heroSliderItem?.TeaserTitle,
                        TeaserText = heroSliderItem?.TeaserText,
                        CtaText = heroSliderItem?.CtaText,
                        CtaUrl = heroSliderItem?.CtaUrl,
                        Image = new ImageViewModel
                        {
                            ImageAlt = heroSliderItem?.Image?.ImageAlt,
                            ImageSrc = heroSliderItem?.Image?.ImageSrc,
                            ImageTitle = heroSliderItem?.Image?.ImageTitle
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
