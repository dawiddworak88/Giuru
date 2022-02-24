using Foundation.Extensions.Models;
using System;

namespace News.Api.ServicesModels.Categories
{
    public class DeleteCategoryServiceModel : BaseServiceModel 
    {
        public Guid? Id { get; set; }
    }
}
