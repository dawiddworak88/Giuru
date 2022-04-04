using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.News.ViewModel
{
    public class NewsItemFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? PreviewImageId { get; set; }
        public Guid? CategoryId { get; set; }
        public string NewsTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string TitleRequiredErrorMessage { get; set; }
        public string CategoryRequiredErrorMessage { get; set; }
        public string DescriptionRequiredErrorMessage { get; set; }
        public string DropFilesLabel { get; set; }
        public string DropOrSelectImagesLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string TitleLabel { get; set; }
        public string ThumbImageLabel { get; set; }
        public string PreviewImageLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string NewsUrl { get; set; }
        public string FilesLabel { get; set; }
        public string ImagesLabel { get; set; }
        public string CategoryLabel { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string IsPublishedLabel { get; set; }
        public string NavigateToNewsLabel { get; set; }
        public string SaveUrl { get; set; }
        public string SaveMediaUrl { get; set; }
        public string SaveText { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<FileViewModel> ThumbnailImages { get; set; }
        public IEnumerable<FileViewModel> PreviewImages { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
    }
}
