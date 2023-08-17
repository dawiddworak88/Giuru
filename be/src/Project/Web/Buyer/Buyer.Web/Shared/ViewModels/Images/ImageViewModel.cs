using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Images
{
    public class ImageViewModel
    {
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
