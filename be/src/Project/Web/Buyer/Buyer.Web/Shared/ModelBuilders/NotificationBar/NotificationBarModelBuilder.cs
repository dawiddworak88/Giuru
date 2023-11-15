using Buyer.Web.Shared.Repositories.GraphQl;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using Foundation.PageContent.Components.Links.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using System.Linq;

namespace Buyer.Web.Shared.ModelBuilders.NotificationBar
{
    public class NotificationBarModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel>
    {
        private readonly IGraphQlRepository _graphQlRepository;
        private readonly IOptionsMonitor<AppSettings> _settings;

        public NotificationBarModelBuilder(IGraphQlRepository graphQlRepository, IOptionsMonitor<AppSettings> settings)
        {
            _graphQlRepository = graphQlRepository;
            _settings = settings;
        }

        public async Task<NotificationBarViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var notificationBarItems = await _graphQlRepository.GetNotificationBar(componentModel.ContentPageKey, componentModel.Language, _settings.CurrentValue.DefaultCulture);

            if(notificationBarItems is not null)
            {
                return new NotificationBarViewModel
                {
                    Items = notificationBarItems.Select(x => new NotificationBarItemViewModel
                    {
                        Icon = x.Icon,
                        Link = new LinkViewModel
                        {
                            Url = x.Link.Href,
                            Text = x.Link.Label,
                            Target = x.Link.Target
                        }
                    })
                };
            }

            return new NotificationBarViewModel();
        }
    }
}
