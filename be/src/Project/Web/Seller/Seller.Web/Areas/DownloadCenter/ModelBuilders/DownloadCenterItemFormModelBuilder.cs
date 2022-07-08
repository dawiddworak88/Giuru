using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.Repositories.Categories;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public DownloadCenterItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<DownloadCenterItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterItemFormViewModel
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
