using Foundation.PageContent.ComponentModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Shared.ComponentModels.Files
{
    public class FilesComponentModel : ComponentModelBase
    {
        public string SearchApiUrl { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
