using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Files
{
    public class FilesViewModel
    {
        public Guid? Id { get; set; }
        public string DisplayedRowsLabel { get; set; }
        public string RowsPerPageLabel { get; set; }
        public string FilesLabel { get; set; }
        public string DownloadLabel { get; set; }
        public string CopyLinkLabel { get; set; }
        public string FilenameLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string SizeLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public string CreatedDateLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public int DefaultPageSize { get; set; }
        public string NoResultsLabel { get; set; }
        public string SearchApiUrl { get; set; }
        public PagedResults<IEnumerable<FileViewModel>> Files { get; set; }
    }
}
