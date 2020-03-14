using Foundation.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;

namespace AspNetCore.Shared.Headers.ModelBuilders
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
