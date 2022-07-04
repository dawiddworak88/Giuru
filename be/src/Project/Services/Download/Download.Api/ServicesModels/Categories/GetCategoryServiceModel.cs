using Foundation.Extensions.Models;
using System;

namespace Download.Api.ServicesModels.Categories
{
    public class GetCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
