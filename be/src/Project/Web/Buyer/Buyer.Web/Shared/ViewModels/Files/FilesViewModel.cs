using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Files
{
    public class FilesViewModel
    {
        public string FilesLabel { get; set; }
        public string DownloadLabel { get; set; }
        public string CopyLinkLabel { get; set; }
        public string FilenameLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string SizeLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public string CreatedDateLabel { get; set; }
        public string AttachmentsLabel { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
