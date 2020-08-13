using Foundation.GenericRepository.Entities;

namespace Catalog.Api.Infrastructure.Brands.Entities
{
    public class BrandTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
