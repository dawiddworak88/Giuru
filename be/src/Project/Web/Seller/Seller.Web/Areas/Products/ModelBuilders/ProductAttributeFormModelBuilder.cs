using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.Configurations;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductAttributeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeFormViewModel>
    {
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributeFormModelBuilder(
            IProductAttributesRepository productAttributesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.productAttributesRepository = productAttributesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductAttributeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductAttributeFormViewModel
            {
                Title = this.productLocalizer.GetString("EditProductAttribute"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.productLocalizer.GetString("EnterProductAttributeName"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                EditUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductAttributesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var productAttribute = await this.productAttributesRepository.GetProductAttributeAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                viewModel.Id = productAttribute.Id;
                viewModel.Name = productAttribute.Name;
            }

            return viewModel;
        }
    }
}
