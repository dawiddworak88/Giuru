using Foundation.Extensions.Models;
using System;

namespace News.Api.ServicesModels.News
{
    public class GetNewsItemFilesServiceModel : PagedBaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
