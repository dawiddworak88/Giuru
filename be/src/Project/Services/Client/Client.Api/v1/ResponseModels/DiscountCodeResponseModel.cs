using System;
using System.Collections.Generic;

namespace Client.Api.v1.ResponseModels
{
    public class DiscountCodeResponseModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
        public int ClientsCount { get; set; }
        public string Version { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
