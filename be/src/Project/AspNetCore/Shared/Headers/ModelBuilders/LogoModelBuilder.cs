using AspNetCore.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;
using Microsoft.Extensions.Localization;

namespace AspNetCore.Shared.Headers.ModelBuilders
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
        private readonly IStringLocalizer<LogoModelBuilder> globalLocalizer;

        public LogoModelBuilder(IStringLocalizer<LogoModelBuilder> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public LogoViewModel BuildModel()
        {
            return new LogoViewModel
            {
                LogoAltLabel = "Logo",
                TargetUrl = "/"
            };
        }
    }
}
