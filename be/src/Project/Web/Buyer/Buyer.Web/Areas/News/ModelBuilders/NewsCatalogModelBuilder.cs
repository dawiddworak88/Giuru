using Buyer.Web.Areas.News.Repositories.Categories;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public NewsCatalogModelBuilder(
            IOptions<AppSettings> options,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<NewsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsCatalogViewModel
            {
                NewsApiUrl = this.linkGenerator.GetPathByAction("Get", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language);
            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }

            return viewModel;
        }
    }
}
