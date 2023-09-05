using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Categories
{
    public class UpdateCategorySchemaServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
