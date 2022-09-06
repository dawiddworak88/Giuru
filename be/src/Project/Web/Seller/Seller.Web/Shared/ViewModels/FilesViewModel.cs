using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
{
    public class FilesViewModel
    {
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
        public PagedResults<IEnumerable<FileViewModel>> Files { get; set; }
    }
}
