using Client.Api.ServicesModels.NotificationTypesApprovals;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.NotificationTypesApprovals
{
    public class GetClientNotificationTypeApprovalsModelValidator : BaseServiceModelValidator<GetClientNotificationTypesApprovalsServiceModel>
    {
        public GetClientNotificationTypeApprovalsModelValidator()
        {
            RuleFor(x => x.ClientId).NotNull();
        }
    }
}
