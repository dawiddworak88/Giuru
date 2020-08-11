using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class MainNavigationModelBuilder : IModelBuilder<MainNavigationViewModel>
    {
        public MainNavigationModelBuilder()
        {
        }

        public MainNavigationViewModel BuildModel()
        {
            var links = new List<LinkViewModel>();

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
