using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Categories
{
    public class GetCategorySchemaServiceModel : BaseServiceModel
    {
        public Guid? CategoryId { get; set; }
    }
}
