using System;

namespace Foundation.Pricing.DiscountCodes
{
    public enum DiscountCodeValidationStatus
    {
        Valid,
        NotFound,
        Disabled,
        NotAssigned,
        ClientUnknown,

        /// <summary>The code cannot be applied, for a reason the caller is not told (what a buyer receives).</summary>
        NotApplicable,

        /// <summary>Applicability could not be determined, so the code must be treated as unverified.</summary>
        Unavailable
    }

    public sealed class DiscountCodeValidation
    {
        public DiscountCodeValidationStatus Status { get; init; }

        /// <summary>The canonical spelling of the code. Set only when <see cref="Status"/> is <see cref="DiscountCodeValidationStatus.Valid"/>.</summary>
        public string DiscountCode { get; init; }

        public bool IsValid => Status is DiscountCodeValidationStatus.Valid;

        public static DiscountCodeValidation Valid(string canonicalDiscountCode)
        {
            return new DiscountCodeValidation { Status = DiscountCodeValidationStatus.Valid, DiscountCode = canonicalDiscountCode };
        }

        public static DiscountCodeValidation Invalid(DiscountCodeValidationStatus status)
        {
            return new DiscountCodeValidation { Status = status };
        }

        /// <summary>
        /// Maps the explicit string status of the Client.Api validation response. A missing or unknown status is not an
        /// answer, so it maps to <see cref="DiscountCodeValidationStatus.Unavailable"/> and the code stays unverified.
        /// </summary>
        public static DiscountCodeValidation FromApiResponse(string status, string discountCode)
        {
            if (!Enum.TryParse<DiscountCodeValidationStatus>(status, ignoreCase: false, out var parsed)
                || !Enum.IsDefined(typeof(DiscountCodeValidationStatus), parsed)
                || parsed == DiscountCodeValidationStatus.Unavailable)
            {
                return Invalid(DiscountCodeValidationStatus.Unavailable);
            }

            return parsed == DiscountCodeValidationStatus.Valid
                ? Valid(discountCode)
                : Invalid(parsed);
        }
    }
}
