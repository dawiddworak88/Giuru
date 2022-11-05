using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Areas.Media.DomainModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ViewModel
{
    public class MediaItemFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string MetaData { get; set; }
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
        public string LatestVersionsLabel { get; set; }
        public string MetaDataLabel { get; set; }
        public string MediaUrl { get; set;}
        public string IdLabel { get; set; }
        public string SaveMediaChunkUrl { get; set; }
        public string SaveMediaChunkCompleteUrl { get; set; }
        public bool IsUploadInChunksEnabled { get; set; }
        public int? ChunkSize { get; set; }
        public string NoGroupsText { get; set; }
        public string GroupsLabel { get; set; }
        public IEnumerable<ListItemViewModel> Groups { get; set; }
        public IEnumerable<MediaItem> Versions { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
    }
}
