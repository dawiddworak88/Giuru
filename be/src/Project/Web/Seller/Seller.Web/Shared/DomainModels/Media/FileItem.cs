using System;

namespace Seller.Web.Shared.DomainModels.Media
{
    public class FileItem
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsProtected { get; set; }
        public string Size { get; set; }
        public string MimeType { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
