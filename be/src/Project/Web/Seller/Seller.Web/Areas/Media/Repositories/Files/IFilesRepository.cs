using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories.Files
{
    public interface IFilesRepository
    {
        Task<Guid> SaveAsync(string token, string language, byte[] file, string filename, string id);
        Task SaveChunkAsync(string token, string language, byte[] file, string filename, int? chunkNumber, Guid uploadId);
        Task<Guid> SaveChunksCompleteAsync(string token, string language, Guid? id, string filename, Guid uploadId);
    }
}
