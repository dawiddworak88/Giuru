using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Services;
using NSubstitute;
using System;
using System.Net;
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
                .Returns(call => Task.FromResult(FindStoredSpelling(storedSpellings, call.ArgAt<string>(3))));

            return repository;
        }

        /// <summary>Like <see cref="Knowing"/>, but the codes are valid only for <paramref name="clientId"/>.</summary>
        public static IDiscountCodeEligibilityRepository KnowingFor(Guid clientId, params string[] storedSpellings)
        {
            var repository = Substitute.For<IDiscountCodeEligibilityRepository>();

            repository.GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(call.ArgAt<Guid?>(2) == clientId ? FindStoredSpelling(storedSpellings, call.ArgAt<string>(3)) : null));

            return repository;
        }

        /// <summary>Fails every check, as the repositories do when Client.Api cannot be reached.</summary>
        public static Func<string, Task<string>> Unreachable()
        {
            return _ => throw new CustomException("Client.Api is unreachable.", (int)HttpStatusCode.ServiceUnavailable);
        }

        private static string FindStoredSpelling(string[] storedSpellings, string requested)
        {
            return Array.Find(storedSpellings, x => string.Equals(x, requested, StringComparison.OrdinalIgnoreCase));
        }
    }
}
