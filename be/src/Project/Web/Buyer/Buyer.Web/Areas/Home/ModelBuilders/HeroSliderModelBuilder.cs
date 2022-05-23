using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.Repositories;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
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
            var viewModel = new HeroSliderViewModel();
            var heroSliderItemViewModels = new List<HeroSliderItemViewModel>();

            var heroSliderItems = await this.heroSliderRepository.GetHeroSliderItemsAsync(componentModel.Language, this.options.Value.DefaultCulture);

            foreach (var heroSliderItem in heroSliderItems.OrEmptyIfNull())
            {
                // TODO: Prepare ViewModel
                // heroSliderItemViewModels.Add(this.GetHeroSliderItem());
            }

            var cornersHeroSliderItem = GetHeroSliderItem(
                this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true)),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Browse"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sectionals.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1600x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1024x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners414x286MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1600x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1024x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners414x286MediaId, true)) }
                });

            var boxspringsHeroSliderItem = GetHeroSliderItem(
                this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true)),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Discover"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Beds.Id }),
                new List<SourceViewModel> 
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1600x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1024x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings414x286MediaId, true, MediaConstants.WebpExtension)) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1600x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1024x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings414x286MediaId, true)) }
                });

            var chairsHeroSliderItem = GetHeroSliderItem(
                this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true)),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Shop"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Chairs.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1600x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1024x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs414x286MediaId, true, MediaConstants.WebpExtension)) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1600x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1024x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs414x286MediaId, true)) }
                });

            var setsHeroSliderItem = GetHeroSliderItem(
                this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true)),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Browse"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sets.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1600x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1024x400MediaId, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets414x286MediaId, true, MediaConstants.WebpExtension)) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1600x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1024x400MediaId, true)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets414x286MediaId, true)) }
                });

            heroSliderItemViewModels.Add(cornersHeroSliderItem);
            heroSliderItemViewModels.Add(boxspringsHeroSliderItem);
            heroSliderItemViewModels.Add(chairsHeroSliderItem);
            heroSliderItemViewModels.Add(setsHeroSliderItem);

            viewModel.Items = heroSliderItemViewModels;

            return viewModel;
        }

        private HeroSliderItemViewModel GetHeroSliderItem(string imageSrc, 
            string imageAlt, 
            string imageTitle, 
            string teaserTitle, 
            string teaserText, 
            string ctaText, 
            string ctaUrl,
            IEnumerable<SourceViewModel> sources)
        {
            return new HeroSliderItemViewModel
            {
                TeaserTitle = teaserTitle,
                TeaserText = teaserText,
                CtaText = ctaText,
                CtaUrl = ctaUrl,
                Image = new ImageViewModel
                { 
                    ImageAlt = imageAlt,
                    ImageSrc = imageSrc,
                    ImageTitle = imageTitle,
                    Sources = sources
                }
            };
        }
    }
}
