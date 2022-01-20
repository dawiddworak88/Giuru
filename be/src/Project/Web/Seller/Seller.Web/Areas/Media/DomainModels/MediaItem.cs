using System;

namespace Seller.Web.Areas.Media.DomainModels
{
    public class MediaItem
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
