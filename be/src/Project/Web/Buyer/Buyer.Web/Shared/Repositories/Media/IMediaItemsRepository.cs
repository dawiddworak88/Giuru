using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Media
{
    public interface IMediaItemsRepository
    {
        Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id);
    }
}
