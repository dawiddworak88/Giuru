using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Seller.Web.Areas.News.ViewModel;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class CategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CategoryFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
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
                SaveText = this.globalLocalizer.GetString("SaveText")
            };

            return viewModel;
        }
    }
}
