using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Seller.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Foundation.PageContent.Components.Headers.Definitions;
using System.Globalization;
using Foundation.Media.Services.MediaServices;

namespace Seller.Web.Shared.ModelBuilders.Headers
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaService mediaService;

        public LogoModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IMediaService mediaService)
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
                LogoUrl = this.mediaService.GetMediaUrl(LogoConstants.LogoMediaId)
            };
        }
    }
}
