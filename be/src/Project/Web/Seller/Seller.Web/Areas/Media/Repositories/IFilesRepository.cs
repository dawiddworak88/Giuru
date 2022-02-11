using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories
{
    public interface IFilesRepository
    {
        Task<Guid> SaveAsync(string token, string language, byte[] file, string filename, string id);
    }
}
