using Buyer.Web.Areas.Content.DomainModel;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.Repositories
{
    public interface ISlugRepository
    {

        Task<Slug> GetPageBySlugAsync(string language, string fallbackLanguage, string slug);
    }
}
