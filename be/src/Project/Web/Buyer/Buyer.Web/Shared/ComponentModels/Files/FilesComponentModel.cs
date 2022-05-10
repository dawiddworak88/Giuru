using Foundation.PageContent.ComponentModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ComponentModels.Files
{
    public class FilesComponentModel : ComponentModelBase
    {
        public IEnumerable<Guid> Files { get; set; }
        public bool? IsAttachments { get; set; }
    }
}
