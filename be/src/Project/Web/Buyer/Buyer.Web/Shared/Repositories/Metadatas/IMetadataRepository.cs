using Buyer.Web.Shared.DomainModels.Metadata;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Metadatas
{
    public interface IMetadataRepository
    {
        Task<Metadata> GetMetadataAsync(string contentPageKey, string language);
    }
}
