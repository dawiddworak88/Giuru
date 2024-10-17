using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.Notification;
using FluentValidation;

namespace Client.Api.Validators.NotificationsType
{
    public class CreateClientNotificationTypeModelValidator : BaseServiceModelValidator<CreateClientNotificationTypeServiceModel>
    {
        public CreateClientNotificationTypeModelValidator()
        { 
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
