using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Images
{
    public class ImageViewModel
    {
        //public Guid? Id { get; set; }
        //public string Original { get; set; }
        //public string Thumbnail { get; set; }

        public string ImageSrc { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
