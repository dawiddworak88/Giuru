using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Categories.Models
{
    public class DeleteCategoryModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
