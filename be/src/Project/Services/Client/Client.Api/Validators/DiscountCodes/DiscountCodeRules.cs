using FluentValidation;
using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Api.Validators.DiscountCodes
{
    public static class DiscountCodeRules
    {
        public static IRuleBuilderOptions<T, string> BeAValidCode<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty()
                .Must(x => string.IsNullOrWhiteSpace(x) is false)
                .Must(x => x is null || x.Trim().Length <= DiscountCodesConstants.CodeMaxLength);
        }

        public static IRuleBuilderOptions<T, string> BeAValidDescription<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(x => x is null || x.Length <= DiscountCodesConstants.DescriptionMaxLength);
        }

        public static IRuleBuilderOptions<T, IEnumerable<Guid>> BeValidClientIds<T>(this IRuleBuilder<T, IEnumerable<Guid>> rule)
        {
            return rule
                .NotNull()
                .Must(x => x is null || x.All(id => id != Guid.Empty));
        }

        public static IRuleBuilderOptions<T, string> BeAValidVersion<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty()
                .Must(x => string.IsNullOrWhiteSpace(x) || TryDecodeVersion(x, out _));
        }

        public static void RequireOrganisation<T>(this AbstractValidator<T> validator) where T : BaseServiceModel
        {
            validator.RuleFor(x => x.OrganisationId)
                .NotNull()
                .Must(x => x.HasValue && x.Value != Guid.Empty);
        }

        public static bool TryDecodeVersion(string version, out byte[] rowVersion)
        {
            rowVersion = null;

            if (string.IsNullOrWhiteSpace(version))
            {
                return false;
            }

            try
            {
                var decoded = Convert.FromBase64String(version);

                if (decoded.Length != DiscountCodesConstants.RowVersionLength)
                {
                    return false;
                }

                rowVersion = decoded;

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
