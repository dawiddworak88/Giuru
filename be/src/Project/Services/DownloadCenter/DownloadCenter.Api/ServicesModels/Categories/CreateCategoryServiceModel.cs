using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.Categories
{
    public class CreateCategoryServiceModel : BaseServiceModel
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
