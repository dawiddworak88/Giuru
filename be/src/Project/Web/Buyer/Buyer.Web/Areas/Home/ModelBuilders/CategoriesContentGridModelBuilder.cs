using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Buyer.Web.Shared.Services.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class CategoriesContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoriesContentGridViewModel>
    {
        private readonly ICatalogService catalogService;
        private readonly IOptions<AppSettings> options;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CategoriesContentGridModelBuilder(ICatalogService catalogService, IOptions<AppSettings> options, IHttpContextAccessor httpContextAccessor)
        {
            this.catalogService = catalogService;
            this.options = options;
            this.httpContextAccessor = httpContextAccessor;
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
                        Url = "#"
                    };

                    if (subCategory.ThumbnailMediaId.HasValue)
                    {
                        carouselItem.ImageUrl = $"{this.httpContextAccessor.HttpContext.Request.Scheme}://{this.options.Value.MediaUrl}{ApiConstants.Media.MediaApiEndpoint}/{subCategory.ThumbnailMediaId.Value}";
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
