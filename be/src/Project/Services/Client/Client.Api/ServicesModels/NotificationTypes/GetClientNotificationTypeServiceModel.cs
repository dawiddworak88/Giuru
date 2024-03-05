using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.NotificationTypes
{
    public class GetClientNotificationTypeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
