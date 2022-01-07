using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.Services.Contents
{
    public interface IContentService
    {
        Task<string> GetAsync(Guid id, string language);
    }
}
