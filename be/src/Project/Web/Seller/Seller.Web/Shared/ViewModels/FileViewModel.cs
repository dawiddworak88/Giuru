using System;

namespace Seller.Web.Shared.ViewModels
{
    public class FileViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
    }
}
