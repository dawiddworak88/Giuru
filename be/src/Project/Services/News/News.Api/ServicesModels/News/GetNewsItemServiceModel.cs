using Foundation.Extensions.Models;
using System;

namespace News.Api.ServicesModels.News
{
    public class GetNewsItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
