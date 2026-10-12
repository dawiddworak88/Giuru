using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class DiscountCodeServiceModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }

        /// <summary>The ids of the still-active clients the code is assigned to. Filled for a single code only.</summary>
        public IEnumerable<Guid> ClientIds { get; set; }

        /// <summary>The number of still-active clients the code is assigned to.</summary>
        public int ClientsCount { get; set; }

        /// <summary>The base64 row version the caller sends back to update or delete the code.</summary>
        public string Version { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
