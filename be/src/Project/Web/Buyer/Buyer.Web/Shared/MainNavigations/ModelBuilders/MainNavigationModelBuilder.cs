using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        public MainNavigationModelBuilder()
        {
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var links = new List<LinkViewModel>();

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
