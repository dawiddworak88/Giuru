using System;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class CompletionDateRequestModel
    {
        public Guid TransportId { get; set; }
        public Guid ConditionId { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? CampaignId { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
