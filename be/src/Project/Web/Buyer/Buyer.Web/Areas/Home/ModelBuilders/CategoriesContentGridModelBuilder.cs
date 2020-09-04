using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Catalogs.Services;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class CategoriesContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoriesContentGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public CategoriesContentGridModelBuilder(
            ICatalogService catalogService, 
            IOptions<AppSettings> options, 
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.catalogService = catalogService;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoriesContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var categories = await this.catalogService.GetCategoriesAsync(componentModel.Language);

            foreach (var category in categories.Where(x => x.Level == CategoriesConstants.FirstLevel))
            {
                var contentGridCarouselItems = new List<ContentGridCarouselItemViewModel>();

                foreach (var subCategory in categories.Where(x => x.Level == CategoriesConstants.SecondLevel && x.ParentId == category.Id))
                {
                    var carouselItem = new ContentGridCarouselItemViewModel
                    { 
                        Id = subCategory.Id,
                        Title = subCategory.Name,
                        ImageAlt = subCategory.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, subCategory.Id })
                    };

                    if (subCategory.ThumbnailMediaId.HasValue)
                    {
                        carouselItem.ImageUrl = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, subCategory.ThumbnailMediaId.Value);
                    }

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new ContentGridItemViewModel { Id = category.Id, Title = category.Name, CarouselItems = contentGridCarouselItems });
            }

            var viewModel = new CategoriesContentGridViewModel
            {
                Items = items
            };

            return viewModel;
        }
    }
}
