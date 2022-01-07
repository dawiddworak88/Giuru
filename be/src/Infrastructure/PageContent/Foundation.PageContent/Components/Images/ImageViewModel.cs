using System.Collections.Generic;

namespace Foundation.PageContent.Components.Images
{
    public class ImageViewModel
    {
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public string ImageTitle { get; set; }
        public string ImageSrcset { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}