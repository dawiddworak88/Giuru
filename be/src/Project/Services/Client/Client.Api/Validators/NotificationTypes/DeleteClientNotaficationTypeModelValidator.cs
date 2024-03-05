using FluentValidation;
using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.NotificationTypes;

namespace Client.Api.Validators.NotificationType
{
    public class DeleteClientNotaficationTypeModelValidator : BaseServiceModelValidator<DeleteClientNotificationTypeServiceModel>
    {
        public DeleteClientNotaficationTypeModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
