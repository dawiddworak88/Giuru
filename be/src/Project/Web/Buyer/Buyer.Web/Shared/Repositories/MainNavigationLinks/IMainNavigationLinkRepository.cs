using Buyer.Web.Shared.DomainModels.MainNavigationLinks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.MainNavigationLinks
{
    public interface IMainNavigationLinkRepository
    {
        Task<IEnumerable<MainNavigationLink>> GetMainNavigationLinksAsync(string contentPageKey, string language, string fallbackLanguage);
    }
}