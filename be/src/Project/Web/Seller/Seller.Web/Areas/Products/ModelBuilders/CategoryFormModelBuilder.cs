using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.Definitons;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class CategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public CategoryFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryFormViewModel
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

            var parentCategories = await this.categoriesRepository.GetAllCategoriesAsync(
                componentModel.Token,
                componentModel.Language,
                null,
                $"{nameof(Category.Level)},{nameof(Category.Name)}");

            if (parentCategories != null)
            {
                viewModel.ParentCategories = parentCategories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
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
                        var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(
                            componentModel.Token, 
                            componentModel.Language,
                            category.ThumbnailMediaId.Value);

                        if (mediaItem != null)
                        {
                            viewModel.Files = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = mediaItem.Id,
                                    Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, mediaItem.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
                                    Name = mediaItem.Name,
                                    MimeType = mediaItem.MimeType,
                                    Filename = mediaItem.Filename,
                                    Extension = mediaItem.Extension
                                }
                            };
                        }
                    }
                }
            }

            return viewModel;
        }
    }
}
