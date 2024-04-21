using Foundation.Extensions.Validators;
using FluentValidation;
using Client.Api.ServicesModels.NotificationTypes;

namespace Client.Api.Validators.NotificationType
{
    public class CreateClientNotificationTypeModelValidator : BaseServiceModelValidator<CreateClientNotificationTypeServiceModel>
    {
        public CreateClientNotificationTypeModelValidator()
        { 
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
