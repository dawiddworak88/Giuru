using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Configurations;
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
        private readonly IMediaService mediaService;
        private readonly IOptions<AppSettings> options;

        public BrandDetailModelBuilder(
            IOptions<AppSettings> options,
            IMediaService mediaService)
        {
            this.options = options;
            this.mediaService = mediaService;
        }

        public async Task<BrandDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new BrandDetailViewModel
            {
                LogoUrl = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, LogoConstants.LogoMediaId)
            };

            return viewModel;
        }
    }
}
