using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Extensions;
using System;
using System.Security.Claims;

namespace Giuru.UnitTests.Extensions
{
    public class ClaimsPrincipalExtensionsTests
    {
        [Fact]
        public void GetClientId_ValidClientIdClaim_ReturnsClientId()
        {
            var clientId = Guid.NewGuid();
            var user = CreateUser(new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, clientId.ToString()));

            Assert.Equal(clientId, user.GetClientId());
        }

        [Fact]
        public void GetClientId_MissingClientIdClaim_ReturnsNull()
        {
            var user = CreateUser();

            Assert.Null(user.GetClientId());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid-client-id")]
        public void GetClientId_InvalidClientIdClaim_ReturnsNull(string claimValue)
        {
            var user = CreateUser(new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, claimValue));

            Assert.Null(user.GetClientId());
        }

        [Fact]
        public void GetClientId_UnrelatedClaims_ReturnsNull()
        {
            var user = CreateUser(new Claim(ClaimTypes.Name, "buyer@example.com"));

            Assert.Null(user.GetClientId());
        }

        private static ClaimsPrincipal CreateUser(params Claim[] claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        }
    }
}