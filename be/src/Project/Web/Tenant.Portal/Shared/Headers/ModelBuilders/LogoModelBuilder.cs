using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Shared.Headers.ViewModels;

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
