using Microsoft.Extensions.Localization;
using NSubstitute;

namespace Giuru.UnitTests.Helpers
{
    public static class TestLocalizer
    {
        /// <summary>A localizer that answers every key with the key itself, so tests can assert on which message was chosen.</summary>
        public static IStringLocalizer<T> Create<T>()
        {
            var localizer = Substitute.For<IStringLocalizer<T>>();

            localizer[Arg.Any<string>()].Returns(call => new LocalizedString(call.Arg<string>(), call.Arg<string>()));
            localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(call => new LocalizedString(call.ArgAt<string>(0), call.ArgAt<string>(0)));

            return localizer;
        }
    }
}
