using Foundation.Pricing.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Orders.Baskets
{
    internal static class DiscountCodeEligibilityStubs
    {
        /// <summary>Treats every requested code as valid, answering with the requested spelling.</summary>
        public static IDiscountCodeEligibilityRepository Accepting()
        {
            var repository = Substitute.For<IDiscountCodeEligibilityRepository>();

            repository.GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(call.ArgAt<string>(3)));

            return repository;
        }

        /// <summary>
        /// Answers with the stored spelling for the given codes, matched ignoring case, and treats
        /// anything else as invalid.
        /// </summary>
        public static IDiscountCodeEligibilityRepository Knowing(params string[] storedSpellings)
        {
            var repository = Substitute.For<IDiscountCodeEligibilityRepository>();

            repository.GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(Array.Find(storedSpellings, x => string.Equals(x, call.ArgAt<string>(3), StringComparison.OrdinalIgnoreCase))));

            return repository;
        }
    }
}
