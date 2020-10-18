using Foundation.PageContent.ComponentModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.Files.ComponentModels
{
    public class FilesComponentModel : ComponentModelBase
    {
        public IEnumerable<Guid> Files { get; set; }
    }
}
