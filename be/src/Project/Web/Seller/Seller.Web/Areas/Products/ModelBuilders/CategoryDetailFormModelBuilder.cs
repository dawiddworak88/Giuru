using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CategoryDetailFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CategoryDetailFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryDetailFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryDetailFormViewModel
            {
                Title = this.productLocalizer.GetString("EditCategory"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                ParentCategoryLabel = this.productLocalizer.GetString("ParentCategory"),
                SelectCategoryLabel = this.productLocalizer.GetString("SelectCategory"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.productLocalizer.GetString("EnterCategoryName"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred")
            };

            var parentCategories = await this.categoriesRepository.GetCategoriesAllAsync(
                componentModel.Token,
                componentModel.Language,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize);

            if (parentCategories != null)
            {
                viewModel.ParentCategories = parentCategories.Select(x => new ParentCategoryViewModel { Id = x.Id, Name = x.Name, Level = x.Level }).OrderBy(x => x.Level).ThenBy(x => x.Name);

                if (componentModel.Id.HasValue)
                {
                }
            }

            return viewModel;
        }
    }
}
