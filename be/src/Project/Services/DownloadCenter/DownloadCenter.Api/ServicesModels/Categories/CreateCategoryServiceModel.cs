using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.Categories
{
    public class CreateCategoryServiceModel : BaseServiceModel
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
