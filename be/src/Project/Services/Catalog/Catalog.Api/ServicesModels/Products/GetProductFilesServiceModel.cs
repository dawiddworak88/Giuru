using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductFilesServiceModel : PagedBaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
