using System;

namespace Identity.Api.v1.Areas.Accounts.ResultModels
{
    public class SellerResultModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
