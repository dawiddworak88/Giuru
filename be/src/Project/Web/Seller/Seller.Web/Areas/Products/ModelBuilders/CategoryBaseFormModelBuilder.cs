using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CategoryBaseFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public CategoryBaseFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
        }

        public async Task<CategoryBaseFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryBaseFormViewModel
            {
                Id = componentModel.Id,
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.productLocalizer.GetString("EditCategory"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                ParentCategoryLabel = this.productLocalizer.GetString("ParentCategory"),
                SelectCategoryLabel = this.productLocalizer.GetString("SelectCategory"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.productLocalizer.GetString("EnterCategoryName"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                CategoryPictureLabel = this.productLocalizer.GetString("CategoryPicture"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "CategoriesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                CategoriesUrl = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCategoriesLabel = this.productLocalizer.GetString("NavigateToCategoriesLabel")
            };

            var parentCategories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language, null, $"{nameof(Category.Level)}");

            if (parentCategories is not null)
            {
                viewModel.ParentCategories = parentCategories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            return viewModel;
        }
    }
}
