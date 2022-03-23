using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Seller.Web.Areas.News.ViewModel;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Seller.Web.Areas.News.Repositories.Categories;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using Newtonsoft.Json;
using System.Globalization;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class CategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoryFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
                Title = this.newsLocalizer.GetString("EditCategory"),
                NameLabel = this.newsLocalizer.GetString("NameLabel"),
                NameRequiredErrorMessage = this.newsLocalizer.GetString("NameRequiredErrorMessage"),
                ParentCategoryLabel = this.newsLocalizer.GetString("ParentCategoryLabel"),
                SelectCategoryLabel = this.newsLocalizer.GetString("SelectCategoryLabel"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "CategoriesApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCategoriesLabel = this.newsLocalizer.GetString("NavigateToCategoriesLabel"),
                CategoriesUrl = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language);
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
