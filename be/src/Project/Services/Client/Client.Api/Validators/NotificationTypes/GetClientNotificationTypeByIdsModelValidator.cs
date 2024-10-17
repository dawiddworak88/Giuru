using Client.Api.ServicesModels.NotificationTypes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.NotificationType
{
    public class GetClientNotificationTypeByIdsModelValidator : BaseServiceModelValidator<GetClientNotificationTypeByIdsServiceModel>
    {
        public GetClientNotificationTypeByIdsModelValidator() 
        { 
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
