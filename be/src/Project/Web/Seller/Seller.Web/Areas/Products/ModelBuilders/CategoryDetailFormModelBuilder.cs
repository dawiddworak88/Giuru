using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.Definitons;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CategoryDetailFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public CategoryDetailFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
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
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                CategoryPictureLabel = this.productLocalizer.GetString("CategoryPicture"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "CategoriesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            var parentCategories = await this.categoriesRepository.GetCategoriesAllAsync(
                componentModel.Token,
                componentModel.Language,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize);

            if (parentCategories != null)
            {
                viewModel.ParentCategories = parentCategories.Select(x => new ParentCategoryViewModel { Id = x.Id, Name = x.Name, Level = x.Level }).OrderBy(x => x.Level).ThenBy(x => x.Name);
            }

            if (componentModel.Id.HasValue)
            {
                var category = await this.categoriesRepository.GetCategoryAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                if (category != null)
                {
                    viewModel.Id = category.Id;
                    viewModel.Name = category.Name;
                    viewModel.ParentCategoryId = category.ParentId;

                    if (category.ThumbnailMediaId.HasValue)
                    {
                        viewModel.Files = new List<FileViewModel>
                        {
                            new FileViewModel
                            { 
                                Id = category.ThumbnailMediaId.Value,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, category.ThumbnailMediaId.Value, Constants.CategoryPreviewMaxWidth, Constants.CategoryPreviewMaxHeight, true)
                            }
                        };
                    }
                }
            }

            return viewModel;
        }
    }
}
