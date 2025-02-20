using Buyer.Web.Shared.DomainModels.GraphQl.FooterLinks;
using Buyer.Web.Shared.DomainModels.GraphQl.MainNavigationLinks;
using Buyer.Web.Shared.DomainModels.GraphQl.NotificationBars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.GraphQl
{
    public interface IGraphQlRepository
    {
        Task<Footer> GetFooterAsync(string language, string fallbackLanguage);
        Task<IEnumerable<NotificationBarItem>> GetNotificationBar(string language, string fallbackLanguage);
        Task<IEnumerable<MainNavigationLink>> GetMainNavigationLinksAsync(string language, string fallbackLanguage);
        Task<string> GetTextAsync(string language, string fallbackLanguage, string attribute);
    }
}
