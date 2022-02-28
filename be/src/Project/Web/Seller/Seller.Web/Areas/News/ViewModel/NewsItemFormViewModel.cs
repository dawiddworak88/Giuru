using System;

namespace Seller.Web.Areas.News.ViewModel
{
    public class NewsItemFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string DropFilesLabel { get; set; }
        public string DropOrSelectImagesLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
    }
}
