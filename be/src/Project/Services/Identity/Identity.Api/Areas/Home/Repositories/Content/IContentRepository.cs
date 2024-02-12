using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.Repositories.Content
{
    public interface IContentRepository
    {
        Task<DomainModels.Content> GetContentAsync(string contentPageKey, string language);
    }
}
