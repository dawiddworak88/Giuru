using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Notification
{
    public class GetClientNotificationTypeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
