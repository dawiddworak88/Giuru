using Foundation.Extensions.Models;
using System;

namespace News.Api.ServicesModels.News
{
    public class DeleteNewsItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
