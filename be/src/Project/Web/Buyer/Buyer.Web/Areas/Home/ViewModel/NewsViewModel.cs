using Buyer.Web.Areas.Home.DomainModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Home.ViewModel
{
    public class NewsViewModel
    {
        public string Title { get; set; }
        public PagedResults<IEnumerable<NewsItemViewModel>> PagedResults { get; set; }
    }
}
