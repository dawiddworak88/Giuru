using System;
using System.Collections.Generic;

namespace Client.Api.v1.RequestModels
{
    public class DiscountCodeRequestModel
    {
        public Guid? Id { get; set; }

        /// <summary>The version the caller last read. Required to update an existing code.</summary>
        public string Version { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }

        /// <summary>The complete list of assigned clients. An empty list assigns the code to nobody.</summary>
        public IEnumerable<Guid> ClientIds { get; set; }
    }
}
