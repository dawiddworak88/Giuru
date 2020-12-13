using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Foundation.Extensions.Services.MediaServices;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Products.Definitons;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public ProductFormModelBuilder(
            IProductsRepository productsRepository,
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductFormViewModel
            {
                Title = this.productLocalizer.GetString("EditProduct"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NameLabel = this.globalLocalizer.GetString("NameLabel"),
                DescriptionLabel = this.globalLocalizer.GetString("DescriptionLabel"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                EnterNameText = this.globalLocalizer.GetString("EnterNameText"),
                EnterSkuText = this.productLocalizer.GetString("EnterSkuText"),
                SkuRequiredErrorMessage = this.productLocalizer.GetString("SkuRequiredErrorMessage"),
                SkuLabel = this.productLocalizer.GetString("SkuLabel"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Save", "ProductApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductPicturesLabel = this.productLocalizer.GetString("ProductPicturesLabel"),
                ProductFilesLabel = this.productLocalizer.GetString("ProductFilesLabel"),
                SelectCategoryLabel = this.productLocalizer.GetString("SelectCategory"),
                SelectPrimaryProductLabel = this.productLocalizer.GetString("SelectPrimaryProduct"),
                IsNewLabel = this.productLocalizer.GetString("IsNew")
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(
                componentModel.Token,
                componentModel.Language,
                true,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize);

            if (categories != null)
            {
                viewModel.Categories = categories.OrderBy(x => x.Level).ThenBy(x => x.Name).Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var primaryProducts = await this.productsRepository.GetAllPrimaryProductsAsync(
                componentModel.Token,
                componentModel.Language,
                componentModel.SellerId,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize);

            if (primaryProducts != null)
            {
                viewModel.PrimaryProducts = primaryProducts.OrderBy(x => x.Name).Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var product = await this.productsRepository.GetProductAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                if (product != null)
                {
                    viewModel.Id = product.Id;
                    viewModel.Name = product.Name;
                    viewModel.Sku = product.Sku;
                    viewModel.Description = product.Description;
                    viewModel.IsNew = product.IsNew;
                    viewModel.CategoryId = product.CategoryId;
                    viewModel.PrimaryProductId = product.PrimaryProductId;

                    if (product.Images != null)
                    {
                        var images = new List<FileViewModel>();

                        foreach (var imageId in product.Images)
                        {
                            images.Add(new FileViewModel 
                            {
                                Id = imageId,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, imageId, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true)
                            });
                        }

                        viewModel.Images = images;
                    }

                    if (product.Files != null)
                    {
                        var files = new List<FileViewModel>();

                        foreach (var fileId in product.Files)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = fileId,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, fileId, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true)
                            });
                        }

                        viewModel.Files = files;
                    }
                }
            }

            return viewModel;
        }
    }
}
