using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.Repositories.Categories;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using System.Globalization;
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
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.categoriesRepository = categoriesRepository;
            this.downloadCenterRepository = downloadCenterRepository;
        }

        public async Task<DownloadCenterItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterItemFormViewModel
            {
                Title = this.globalLocalizer.GetString("EditDownloadCenter"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NavigateToDownloadCenterLabel = this.downloadCenterLocalizer.GetString("NavigateToDownloadCenter"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                DownloadCenterUrl = this.linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                SelectCategoryLabel = this.downloadCenterLocalizer.GetString("SelectCategory"),
                OrderLabel = this.globalLocalizer.GetString("Order"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
            };

            var categories = await this.categoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var downloadCenterItem = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterItem is not null)
                {
                    viewModel.Id = downloadCenterItem.Id;
                    viewModel.CategoryId = downloadCenterItem.CategoryId;
                    viewModel.Order = downloadCenterItem.Order;
                }
            }

            return viewModel;
        }
    }
}
