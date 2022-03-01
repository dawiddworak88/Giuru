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
        public string TitleLabel { get; set; }
        public string HeroImageLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string NewsUrl { get; set; }
        public string IsNewLabel { get; set; }
        public string FilesLabel { get; set; }
        public string ImagesLabel { get; set; }
        public string IsPublishedLabel { get; set; }
        public string NavigateToNewsLabel { get; set; }
        public string SaveUrl { get; set; }
        public string SaveMediaUrl { get; set; }
        public string SaveText { get; set; }
    }
}
