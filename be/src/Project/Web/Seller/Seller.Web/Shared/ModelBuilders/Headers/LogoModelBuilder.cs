using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Seller.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.Components.Headers.Definitions;
using System.Globalization;

namespace Seller.Web.Shared.ModelBuilders.Headers
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaHelperService mediaService;

        public LogoModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IMediaHelperService mediaService)
        {
            this.globalLocalizer = globalLocalizer;
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.mediaService = mediaService;
        }

        public LogoViewModel BuildModel()
        {
            return new LogoViewModel
            {
                LogoAltLabel = this.globalLocalizer.GetString("Logo"),
                TargetUrl = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                LogoUrl = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, LogoConstants.LogoMediaId, true)
            };
        }
    }
}
