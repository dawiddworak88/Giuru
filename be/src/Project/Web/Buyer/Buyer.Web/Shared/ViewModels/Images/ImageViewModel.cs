using System;

namespace Buyer.Web.Shared.ViewModels.Images
{
    public class ImageViewModel
    {
        public Guid? Id { get; set; }
        public string Original { get; set; }
        public string Thumbnail { get; set; }
    }
}
