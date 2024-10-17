using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.NotificationTypes
{
    public class CreateClientNotificationTypeServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
    }
}
