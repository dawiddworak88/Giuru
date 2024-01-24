using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.Notification;
using FluentValidation;

namespace Client.Api.Validators.Notifications
{
    public class GetClientNotificationTypeModelValidator : BaseServiceModelValidator<GetClientNotificationTypeServiceModel>
    {
        public GetClientNotificationTypeModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
