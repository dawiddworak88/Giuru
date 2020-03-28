using Feature.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;

namespace Account.Shared.Headers.ModelBuilders
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
