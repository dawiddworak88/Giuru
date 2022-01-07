using System;

namespace Buyer.Web.Shared.DomainModels.Categories
{
    public class CatalogCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public int Level { get; set; }
        public Guid? ThumbnailMediaId { get; set; }
    }
}
