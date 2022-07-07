using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Download.Repositories.Categories;
using Seller.Web.Areas.Download.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.ModelBuilders
{
    public class DownloadFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadResources> downloadLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public DownloadFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadResources> downloadLocalizer,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadLocalizer = downloadLocalizer;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<DownloadFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadFormViewModel
            {

            };

            var categories = await this.categoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            return viewModel;
        }
    }
}
