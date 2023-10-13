using Buyer.Web.Shared.DomainModels.GraphQl;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.GraphQl
{
    public interface IGraphQlRepository
    {
        Task<Footer> GetFooterAsync(string contentPageKey, string language, string fallbackLanguage);
    }
}
