using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class GetDiscountCodeClientOptionsModelValidator : BaseServiceModelValidator<GetDiscountCodeClientOptionsServiceModel>
    {
        public GetDiscountCodeClientOptionsModelValidator()
        {
            this.RequireOrganisation();
        }
    }
}
