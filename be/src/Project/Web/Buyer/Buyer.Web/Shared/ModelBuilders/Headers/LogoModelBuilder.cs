using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Foundation.PageContent.Components.Headers.Definitions;
using Foundation.Media.Services.MediaServices;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaService mediaService;

        public LogoModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator,
            IMediaService mediaService)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.mediaService = mediaService;
        }

        public LogoViewModel BuildModel()
        {
            return new LogoViewModel
            {
                LogoAltLabel = this.globalLocalizer.GetString("Logo"),
                TargetUrl = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                LogoUrl = this.mediaService.GetMediaUrl(LogoConstants.LogoMediaId)
            };
        }
    }
}
