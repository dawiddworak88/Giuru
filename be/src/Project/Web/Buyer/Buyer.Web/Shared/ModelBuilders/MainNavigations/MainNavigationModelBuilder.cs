using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.MainNavigationLinks;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Elastic.CommonSchema;

namespace Buyer.Web.Shared.ModelBuilders.MainNavigations
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IOptionsMonitor<AppSettings> _settings;
        private readonly IMainNavigationLinkRepository _mainNavigationLinkRepository;

        public MainNavigationModelBuilder(
            IOptionsMonitor<AppSettings> settings,
            IMainNavigationLinkRepository mainNavigationLinkRepository)
        {
            _settings = settings;
            _mainNavigationLinkRepository = mainNavigationLinkRepository;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var mainNavigationLinks = await _mainNavigationLinkRepository.GetMainNavigationLinksAsync(componentModel.ContentPageKey, componentModel.Language, _settings.CurrentValue.DefaultCulture);

            return new MainNavigationViewModel
            {
                Links = mainNavigationLinks.OrEmptyIfNull().Select(x => new LinkViewModel
                {
                    Url = x.Href,
                    Text = x.Label,
                    Target = x.Taget
                })
            };
        }
    }
}
