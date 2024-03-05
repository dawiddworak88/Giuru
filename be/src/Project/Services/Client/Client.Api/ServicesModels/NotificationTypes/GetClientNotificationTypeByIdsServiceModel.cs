using System.Collections.Generic;
using System;
using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.NotificationTypes
{
    public class GetClientNotificationTypeByIdsServiceModel : PagedBaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
