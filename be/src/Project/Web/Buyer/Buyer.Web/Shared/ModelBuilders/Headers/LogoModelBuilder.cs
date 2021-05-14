using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Foundation.Extensions.Services.MediaServices;
using System.Globalization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Foundation.PageContent.Components.Headers.Definitions;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaHelperService mediaService;
        private readonly ICdnService cdnService;

        public LogoModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IMediaHelperService mediaService,
            ICdnService cdnService)
        {
            this.globalLocalizer = globalLocalizer;
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.mediaService = mediaService;
            this.cdnService = cdnService;
        }

        public LogoViewModel BuildModel()
        {
            return new LogoViewModel
            {
                LogoAltLabel = this.globalLocalizer.GetString("Logo"),
                TargetUrl = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                LogoUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, LogoConstants.LogoMediaId, true))
            };
        }
    }
}
