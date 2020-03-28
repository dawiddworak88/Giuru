using Feature.PageContent.Shared.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;

namespace Tenant.Portal.Shared.Headers.ModelBuilders
{
    public class LogoModelBuilder : IModelBuilder<LogoViewModel>
    {
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
