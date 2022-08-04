using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterCategoryFile
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> CategoriesIds { get; set; }
    }
}
