using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SaveClientRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? OrganisationId { get; set; }
        public IEnumerable<string> MarketingApprovals { get; set; }
    }
}
