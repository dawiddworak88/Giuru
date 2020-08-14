using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.Services.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class CategoriesContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoriesContentGridViewModel>
    {
        private readonly ICatalogService catalogService;

        public CategoriesContentGridModelBuilder(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        public async Task<CategoriesContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var items = new List<ContentGridItemViewModel>();

            var categories = await this.catalogService.GetCategoriesAsync(componentModel.Language);

            foreach (var category in categories.Where(x => x.Level == CategoriesConstants.FirstLevel))
            {
                var contentGridCarouselItems = new List<ContentGridCarouselItemViewModel>();

                foreach (var subCategory in categories.Where(x => x.Level == CategoriesConstants.SecondLevel))
                {
                    var carouselItem = new ContentGridCarouselItemViewModel
                    { 
                        Id = subCategory.Id,
                        Title = subCategory.Name,
                        ImageAlt = subCategory.Name,
                        ImageUrl = "#",
                        Url = "#"
                    };

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
