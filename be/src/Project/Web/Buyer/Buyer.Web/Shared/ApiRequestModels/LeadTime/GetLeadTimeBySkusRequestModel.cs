using Foundation.GenericRepository.Definitions;

namespace Buyer.Web.Shared.ApiRequestModels.LeadTime
{
    public class GetLeadTimeBySkusRequestModel
    {
        public string Skus { get; set; }
        public int PageIndex { get; set; } = Constants.DefaultPageIndex;
        public int PageSize { get; set; } = Constants.MaxItemsPerPage;
    }
}
