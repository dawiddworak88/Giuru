using System.Collections.Generic;
using System;
using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.Notification
{
    public class GetClientNotificationTypeByIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}
