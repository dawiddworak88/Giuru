using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateCategoryFormModelBuilder : IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public DuplicateCategoryFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(DuplicateCategoryComponentModel componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
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

            if (componentModel.Id.HasValue)
            {
                var category = await this.categoriesRepository.GetCategoryAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (category is not null)
                {
                    viewModel.Name = $"{category.Name} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.ParentCategoryId = category.ParentId;

                    if (category.ThumbnailMediaId.HasValue)
                    {
                        var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token,componentModel.Language,category.ThumbnailMediaId.Value);

                        if (mediaItem is not null)
                        {
                            viewModel.Files = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = mediaItem.Id,
                                    Url = this.mediaService.GetMediaUrl(mediaItem.Id, Constants.PreviewMaxWidth),
                                    Name = mediaItem.Name,
                                    MimeType = mediaItem.MimeType,
                                    Filename = mediaItem.Filename,
                                    Extension = mediaItem.Extension
                                }
                            };
                        }
                    }

                    var categorySchema = await this.categoriesRepository.GetCategorySchemaAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                    if (categorySchema is not null)
                    {
                        viewModel.Schema = categorySchema.Schema;
                        viewModel.UiSchema = categorySchema.UiSchema;
                    }
                }
            }

            return viewModel;
        }
    }
}