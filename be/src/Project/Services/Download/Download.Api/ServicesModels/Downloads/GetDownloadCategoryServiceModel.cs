using Foundation.Extensions.Models;
using System;

namespace Download.Api.ServicesModels.Downloads
{
    public class GetDownloadCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
