using Foundation.Pricing.DiscountCodes;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Helpers
{
    public static class DiscountCodeValidators
    {
        /// <summary>A validator that accepts every code and answers with the code as the caller spelled it, trimmed.</summary>
        public static IDiscountCodeValidator AlwaysValid()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();

            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(DiscountCodeValidation.Valid(call.ArgAt<string>(1).Trim())));

            return validator;
        }

        /// <summary>A validator that answers every code with the given status.</summary>
        public static IDiscountCodeValidator Returning(DiscountCodeValidationStatus status)
        {
            var validator = Substitute.For<IDiscountCodeValidator>();

            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(status)));

            return validator;
        }
    }
}
