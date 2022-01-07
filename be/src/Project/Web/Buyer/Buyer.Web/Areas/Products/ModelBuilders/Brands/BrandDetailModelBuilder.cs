using Buyer.Web.Shared.Repositories.Brands;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Headers.Definitions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IBrandRepository brandRepository;
        private readonly IMediaHelperService mediaService;
        private readonly IOptions<AppSettings> options;
        private readonly ICdnService cdnService;

        public BrandDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IBrandRepository brandRepository,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            ICdnService cdnService)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.brandRepository = brandRepository;
            this.options = options;
            this.mediaService = mediaService;
            this.cdnService = cdnService;
        }

        public async Task<BrandDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new BrandDetailViewModel
            {
                LogoUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, LogoConstants.LogoMediaId))
            };

            var brand = await this.brandRepository.GetBrandAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (brand != null)
            {
                viewModel.Name = brand.Name;
                viewModel.Description = brand.Description;
                viewModel.Files = await this.filesModelBuilder.BuildModelAsync(new FilesComponentModel { Id = componentModel.Id, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language, Token = componentModel.Token, Files = brand.Files });
            }

            return viewModel;
        }
    }
}
