using Seller.Web.Areas.Media.DomainModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ViewModel
{
    public class EditMediaFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MediaItemsLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string DeleteLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public string DropOrSelectImagesLabel { get; set; }
        public string SaveMediaUrl { get; set; }
        public string UpdateMediaVersionUrl { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string BackToMediaText { get; set; }
        public string SaveMediaText { get; set; } 
        public IEnumerable<MediaItem> Versions { get; set; }
    }
}
