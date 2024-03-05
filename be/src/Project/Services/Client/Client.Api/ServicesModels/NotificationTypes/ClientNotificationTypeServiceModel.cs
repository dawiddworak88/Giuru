using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.NotificationTypes
{
    public class ClientNotificationTypeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
