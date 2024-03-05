using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.NotificationTypes
{
    public class DeleteClientNotificationTypeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
