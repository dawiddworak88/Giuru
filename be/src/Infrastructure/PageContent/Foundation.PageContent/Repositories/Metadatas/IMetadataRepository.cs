using Foundation.PageContent.DomainModels.Metadatas;
using System.Threading.Tasks;

namespace Foundation.PageContent.Repositories.Metadatas 
{ 

    public interface IMetadataRepository
    {
        Task<Metadata> GetMetadataAsync(string contentPageKey, string language);
    }
}
