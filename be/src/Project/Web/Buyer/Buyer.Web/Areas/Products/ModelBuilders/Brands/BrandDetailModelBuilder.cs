using Buyer.Web.Areas.Products.Repositories.Brands;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Files.ComponentModels;
using Buyer.Web.Shared.Files.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Headers.Definitions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IBrandRepository brandRepository;
        private readonly IMediaHelperService mediaService;
        private readonly IOptions<AppSettings> options;

        public BrandDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IBrandRepository brandRepository,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.brandRepository = brandRepository;
            this.options = options;
            this.mediaService = mediaService;
        }

        public async Task<BrandDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new BrandDetailViewModel
            {
                LogoUrl = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, LogoConstants.LogoMediaId)
            };

            var brand = await this.brandRepository.GetBrandAsync(componentModel.Id, componentModel.Token);

            if (brand != null)
            {
                viewModel.Name = brand.Name;
                viewModel.Description = brand.Description;
            }

            viewModel.Files = await this.filesModelBuilder.BuildModelAsync(new FilesComponentModel { Id = componentModel.Id, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language, Token = componentModel.Token, Files = brand.Files });

            return viewModel;
        }
    }
}
