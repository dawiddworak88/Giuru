using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Download.Repositories.Categories;
using Seller.Web.Areas.Download.ViewModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.ModelBuilders
{
    public class CategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoryFormModelBuilder(
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

        public async Task<CategoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.downloadCenterLocalizer.GetString("EditCategory"),
                NameLabel = this.downloadCenterLocalizer.GetString("NameLabel"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                ParentCategoryLabel = this.downloadCenterLocalizer.GetString("ParentCategoryLabel"),
                SelectCategoryLabel = this.downloadCenterLocalizer.GetString("SelectCategoryLabel"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "CategoriesApi", new { Area = "Download", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCategoriesLabel = this.downloadCenterLocalizer.GetString("NavigateToCategoriesLabel"),
                CategoriesUrl = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "Download", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred")
            };

            var categories = await this.categoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.ParentCategories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var category = await this.categoriesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (category is not null)
                {
                    viewModel.Id = category.Id;
                    viewModel.Name = category.Name;
                    viewModel.ParentCategoryId = category.ParentCategoryId;
                }
            }

            return viewModel;
        }
    }
}
