using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.Repositories.Categories;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
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
                NameLabel = this.globalLocalizer.GetString("Name"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                ParentCategoryLabel = this.downloadCenterLocalizer.GetString("ParentCategory"),
                SelectCategoryLabel = this.downloadCenterLocalizer.GetString("SelectCategory"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "CategoriesApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCategoriesLabel = this.downloadCenterLocalizer.GetString("NavigateToCategoriesLabel"),
                CategoriesUrl = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                FilesLabel = this.globalLocalizer.GetString("Files"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile")
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
