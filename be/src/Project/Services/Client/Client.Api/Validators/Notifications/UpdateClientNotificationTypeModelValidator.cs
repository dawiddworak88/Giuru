using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.Notification;
using FluentValidation;

namespace Client.Api.Validators.Notifications
{
    public class UpdateClientNotificationTypeModelValidator : BaseServiceModelValidator<UpdateClientNotificationTypeServiceModel>
    {
        public UpdateClientNotificationTypeModelValidator()
        { 
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
