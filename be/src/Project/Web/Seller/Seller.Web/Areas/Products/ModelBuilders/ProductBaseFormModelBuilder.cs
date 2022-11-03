using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductBaseFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductBaseFormViewModel>
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IClientGroupsRepository clientGroupsRepository;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductBaseFormModelBuilder(
            ICategoriesRepository categoriesRepository,
            IClientGroupsRepository clientGroupsRepository,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.categoriesRepository = categoriesRepository;
            this.clientGroupsRepository = clientGroupsRepository;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductBaseFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductBaseFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
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
                SaveMediaChunkUrl = this.linkGenerator.GetPathByAction("PostChunk", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkCompleteUrl = this.linkGenerator.GetPathByAction("PostChunksComplete", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                IsUploadInChunksEnabled = true,
                ChunkSize = MediaConstants.DefaultChunkSize,
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductPicturesLabel = this.productLocalizer.GetString("ProductPicturesLabel"),
                ProductFilesLabel = this.productLocalizer.GetString("ProductFilesLabel"),
                SelectCategoryLabel = this.productLocalizer.GetString("SelectCategory"),
                SelectPrimaryProductLabel = this.productLocalizer.GetString("SelectPrimaryProduct"),
                IsNewLabel = this.productLocalizer.GetString("IsNew"),
                IsPublishedLabel = this.productLocalizer.GetString("IsPublished"),
                GetCategorySchemaUrl = this.linkGenerator.GetPathByAction("Get", "CategorySchemasApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductsUrl = this.linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                EanLabel = this.globalLocalizer.GetString("Ean"),
                NavigateToProductsLabel = this.productLocalizer.GetString("NavigateToProductsLabel"),
                ProductsSuggestionUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                NoGroupsText = this.globalLocalizer.GetString("NoGroupsText"),
                GroupsLabel = this.globalLocalizer.GetString("Groups")
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language, true, $"{nameof(Category.Level)}");

            if (categories is not null)
            {
                viewModel.Categories = categories.OrderBy(x => x.Level).ThenBy(x => x.Name).Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var primaryProducts = await this.productsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, null, false, componentModel.SellerId, Constants.ProductsSuggestionDefaultPageIndex, Constants.ProductsSuggestionDefaultItemsPerPage, $"{nameof(Product.Name)} ASC");

            if (primaryProducts is not null)
            {
                viewModel.PrimaryProducts = primaryProducts.Data.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var groups = await this.clientGroupsRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (groups is not null)
            {
                viewModel.Groups = groups.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            return viewModel;
        }
    }
}
