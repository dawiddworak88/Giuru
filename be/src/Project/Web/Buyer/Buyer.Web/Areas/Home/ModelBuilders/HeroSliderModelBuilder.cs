using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Shared.Configurations;
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
        private readonly IStringLocalizer<HeroSliderResources> heroSliderLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;

        public HeroSliderModelBuilder(IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IStringLocalizer<HeroSliderResources> heroSliderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.mediaService = mediaService;
            this.heroSliderLocalizer = heroSliderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HeroSliderViewModel();

            var heroSliderItems = new List<HeroSliderItemViewModel>();

            var cornersHeroSliderItem = GetHeroSliderItem(
                this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                this.heroSliderLocalizer.GetString("BrowseCorners"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Browse"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sectionals.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1600x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1024x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners414x286MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.CornersMediaId, true) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1600x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners1024x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Corners414x286MediaId, true) }
                });

            var boxspringsHeroSliderItem = GetHeroSliderItem(
                this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                this.heroSliderLocalizer.GetString("DiscoverBeds"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Discover"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Beds.Id }),
                new List<SourceViewModel> 
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1600x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1024x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings414x286MediaId, true, MediaConstants.WebpExtension) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BoxspringsMediaId, true) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1600x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings1024x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Boxsprings414x286MediaId, true) }
                });

            var chairsHeroSliderItem = GetHeroSliderItem(
                this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                this.heroSliderLocalizer.GetString("ShopChairs"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Shop"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Chairs.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1600x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1024x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs414x286MediaId, true, MediaConstants.WebpExtension) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.ChairsMediaId, true) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1600x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs1024x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Chairs414x286MediaId, true) }
                });

            var setsHeroSliderItem = GetHeroSliderItem(
                this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                this.heroSliderLocalizer.GetString("BrowseSets"),
                string.Empty,
                this.heroSliderLocalizer.GetString("Browse"),
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sets.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1600x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1024x400MediaId, true, MediaConstants.WebpExtension) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets414x286MediaId, true, MediaConstants.WebpExtension) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.SetsMediaId, true) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1600x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets1024x400MediaId, true) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.Sets414x286MediaId, true) }
                });

            heroSliderItems.Add(cornersHeroSliderItem);
            heroSliderItems.Add(boxspringsHeroSliderItem);
            heroSliderItems.Add(chairsHeroSliderItem);
            heroSliderItems.Add(setsHeroSliderItem);

            viewModel.Items = heroSliderItems;

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
