using Buyer.Web.Areas.Home.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        private readonly IStringLocalizer<HeroSliderResources> heroSliderLocalizer;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public HeroSliderModelBuilder(IMediaService mediaService,
            IStringLocalizer<HeroSliderResources> heroSliderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.mediaService = mediaService;
            this.heroSliderLocalizer = heroSliderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HeroSliderViewModel();

            var heroSliderItems = new List<HeroSliderItemViewModel>();

            var cornersHeroSliderItem = GetHeroSliderItem(
                mediaService.GetMediaUrl(HeroSliderItemConstants.Media.CornersMediaId),
                heroSliderLocalizer.GetString("BrowseCorners"),
                heroSliderLocalizer.GetString("BrowseCorners"),
                heroSliderLocalizer.GetString("BrowseCorners"),
                string.Empty,
                heroSliderLocalizer.GetString("Browse"),
                linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sectionals.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.CornersMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners414x286MediaId) },
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.CornersMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Corners414x286MediaId) }
                });

            var boxspringsHeroSliderItem = GetHeroSliderItem(
                mediaService.GetMediaUrl(HeroSliderItemConstants.Media.BoxspringsMediaId),
                heroSliderLocalizer.GetString("DiscoverBeds"),
                heroSliderLocalizer.GetString("DiscoverBeds"),
                heroSliderLocalizer.GetString("DiscoverBeds"),
                string.Empty,
                heroSliderLocalizer.GetString("Discover"),
                linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Beds.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.BoxspringsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings414x286MediaId) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.BoxspringsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Boxsprings414x286MediaId) }
                });

            var chairsHeroSliderItem = GetHeroSliderItem(
                mediaService.GetMediaUrl(HeroSliderItemConstants.Media.ChairsMediaId),
                heroSliderLocalizer.GetString("ShopChairs"),
                heroSliderLocalizer.GetString("ShopChairs"),
                heroSliderLocalizer.GetString("ShopChairs"),
                string.Empty,
                heroSliderLocalizer.GetString("Shop"),
                linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Chairs.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.ChairsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs414x286MediaId) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.ChairsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Chairs414x286MediaId) }
                });

            var setsHeroSliderItem = GetHeroSliderItem(
                mediaService.GetMediaUrl(HeroSliderItemConstants.Media.SetsMediaId),
                heroSliderLocalizer.GetString("BrowseSets"),
                heroSliderLocalizer.GetString("BrowseSets"),
                heroSliderLocalizer.GetString("BrowseSets"),
                string.Empty,
                heroSliderLocalizer.GetString("Browse"),
                linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sets.Id }),
                new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.SetsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets414x286MediaId) },

                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.SetsMediaId) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets1600x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets1024x400MediaId) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = mediaService.GetMediaUrl(HeroSliderItemConstants.Media.Sets414x286MediaId) }
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
