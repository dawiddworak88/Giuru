using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.DomainModels
{
    public class Test
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
