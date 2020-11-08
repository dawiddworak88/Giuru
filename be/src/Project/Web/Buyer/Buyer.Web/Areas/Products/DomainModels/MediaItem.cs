using System;

namespace Buyer.Web.Shared.Brands.DomainModels
{
    public class MediaItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public bool IsProtected { get; set; }
        public long Size { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
