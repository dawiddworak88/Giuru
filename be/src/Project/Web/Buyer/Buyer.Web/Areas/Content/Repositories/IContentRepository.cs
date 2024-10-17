using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.Repositories
{
    public interface IContentRepository
    {
        Task<DomainModel.Content> GetContentPageBySlugAsync(string language, string fallbackLanguage, string slug);
    }
}
