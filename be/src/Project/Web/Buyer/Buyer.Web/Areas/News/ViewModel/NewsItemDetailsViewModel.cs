using Buyer.Web.Shared.ViewModels.Files;
using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemDetailsViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string PreviewImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string FilesLabel { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SourceViewModel> PreviewImages { get; set; }
        public FilesViewModel Files { get; set; }
    }
}
