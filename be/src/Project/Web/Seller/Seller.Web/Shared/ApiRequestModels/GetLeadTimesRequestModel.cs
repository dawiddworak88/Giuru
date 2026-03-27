using Foundation.GenericRepository.Definitions;
using System;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class GetLeadTimesRequestModel
    {
        public Guid CustomerId { get; set; }
        public string Skus { get; set; }
        public int PageIndex { get; set; } = Constants.DefaultPageIndex;
        public int PageSize { get; set; } = Constants.MaxItemsPerPage;
    }
}
