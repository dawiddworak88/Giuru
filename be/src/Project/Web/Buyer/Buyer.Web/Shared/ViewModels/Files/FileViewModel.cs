using System;

namespace Buyer.Web.Shared.ViewModels.Files
{
    public class FileViewModel
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsProtected { get; set; }
        public string Size { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
