using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// Managing discount codes, assigning them to clients and asking whether a code can be applied - all through
    /// Client.Api, which owns that data. Every test works for a seller organisation of its own.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeManagementTests
    {
        private const string Code = "SUMMER25";

        private readonly ApiFixture _apiFixture;

        public DiscountCodeManagementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        // ---- Management ----

        [Fact]
        public async Task CreateCode_StoresItAndListsItForTheSeller()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var id = await seller.CreateCodeAsync(Code, description: "Summer sale");

            var codes = await seller.ListCodesAsync();
            var stored = Assert.Single(codes);
            Assert.Equal(id, stored.Id);
            Assert.Equal(Code, stored.Code);
            Assert.Equal("Summer sale", stored.Description);
            Assert.False(stored.IsDisabled);
        }

        [Theory]
        [InlineData("SUMMER25")]
        [InlineData("summer25")]
        [InlineData("  Summer25  ")]
        public async Task CreateCode_WhenTheSameCodeExistsIgnoringCaseAndPadding_Returns409(string duplicate)
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            await seller.CreateCodeAsync(Code);

            var response = await seller.TryCreateCodeAsync(duplicate);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Single(await seller.ListCodesAsync());
        }

        [Fact]
        public async Task CreateCode_ForConcurrentRequestsOfTheSameCode_StoresOneAndNeverFailsWithAServerError()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var responses = await Task.WhenAll(Enumerable.Range(0, 6).Select(_ => seller.TryCreateCodeAsync(Code)));

            Assert.Single(responses, x => x.StatusCode == HttpStatusCode.OK);
            Assert.All(responses.Where(x => x.StatusCode != HttpStatusCode.OK), x => Assert.Equal(HttpStatusCode.Conflict, x.StatusCode));
            Assert.Single(await seller.ListCodesAsync());
        }

        [Fact]
        public async Task CreateCode_TheSameTextForTwoSellers_IsAllowed()
        {
            var first = await DiscountSeller.CreateAsync(_apiFixture);
            var second = await DiscountSeller.CreateAsync(_apiFixture);

            await first.CreateCodeAsync(Code);
            await second.CreateCodeAsync(Code);

            Assert.Single(await first.ListCodesAsync());
            Assert.Single(await second.ListCodesAsync());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateCode_WithoutAText_Returns422(string code)
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.TryCreateCodeAsync(code);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task CreateCode_LongerThanAllowed_Returns422()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.TryCreateCodeAsync(new string('A', 65));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task CreateCode_WithADescriptionLongerThanAllowed_Returns422()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.TryCreateCodeAsync(Code, new string('d', 257));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task CreateCode_ForATokenWithoutAUsableOrganisation_IsRejected()
        {
            var token = await _apiFixture.GetTokenAsync($"seller-{Guid.NewGuid():N}@tests.com", "Seller", Guid.Empty);
            var clientApi = _apiFixture.CreateClientApiClient(token);

            var response = await clientApi.PostForResponseAsync(
                ApiConstants.Client.DiscountCodesApiEndpoint,
                new DiscountCodeRequestModel { Code = Code });

            // An empty organisation id is not an organisation: the code must not be created for nobody.
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCode_ChangesTheDescriptionAndTheDisabledFlagButNeverTheCode()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var id = await seller.CreateCodeAsync(Code, description: "Before");

            var response = await seller.UpdateCodeAsync(id, "After", isDisabled: true, code: "RENAMED");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stored = Assert.Single(await seller.ListCodesAsync());
            Assert.Equal(Code, stored.Code);
            Assert.Equal("After", stored.Description);
            Assert.True(stored.IsDisabled);
        }

        [Fact]
        public async Task UpdateCode_ThatDoesNotExist_Returns404()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.UpdateCodeAsync(Guid.NewGuid(), "x", isDisabled: false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCode_DeactivatesItAndItsAssignmentsTogether()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            var response = await seller.DeleteCodeAsync(codeId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(await seller.ListCodesAsync());
            Assert.Equal("NotFound", (await client.ValidateAsSellerAsync(Code)).Status);

            // A deleted code never shows up among a client's assignments.
            Assert.Empty((await seller.GetClientAsync(client.Id)).DiscountCodeIds);
        }

        [Fact]
        public async Task DeleteCode_ThatDoesNotExist_Returns404()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.DeleteCodeAsync(Guid.NewGuid());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateCode_AfterItWasDeleted_IsAllowedAndDoesNotInheritTheOldAssignments()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var oldId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { oldId });
            await seller.DeleteCodeAsync(oldId);

            var newId = await seller.CreateCodeAsync(Code);

            Assert.NotEqual(oldId, newId);
            Assert.Equal("NotAssigned", (await client.ValidateAsSellerAsync(Code)).Status);
            Assert.Empty((await seller.GetClientAsync(client.Id)).DiscountCodeIds);
        }

        // ---- Seller isolation ----

        [Fact]
        public async Task AnotherSeller_CannotListReadEditOrDeleteTheCodesOfASeller()
        {
            var owner = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await owner.CreateCodeAsync(Code);

            Assert.Empty(await other.ListCodesAsync());

            var read = await other.ClientApi.GetForResponseAsync($"{ApiConstants.Client.DiscountCodesApiEndpoint}/{codeId}");
            Assert.Equal(HttpStatusCode.NotFound, read.StatusCode);

            Assert.Equal(HttpStatusCode.NotFound, (await other.UpdateCodeAsync(codeId, "hijacked", isDisabled: true)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await other.DeleteCodeAsync(codeId)).StatusCode);

            var stillThere = Assert.Single(await owner.ListCodesAsync());
            Assert.Equal(codeId, stillThere.Id);
            Assert.Null(stillThere.Description);
            Assert.False(stillThere.IsDisabled);
        }

        [Fact]
        public async Task AnotherSeller_CannotRetrieveTheAssignmentsOfASellersClientsThroughTheClientList()
        {
            var owner = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await owner.CreateCodeAsync(Code);
            var client = await owner.CreateClientAsync(new[] { codeId });

            var list = await other.ClientApi.GetAsync<Foundation.GenericRepository.Paginations.PagedResults<System.Collections.Generic.IEnumerable<ClientResponseModel>>>(
                $"{ApiConstants.Client.ClientsApiEndpoint}?pageIndex=1&itemsPerPage=100");

            Assert.DoesNotContain(list.Data ?? Enumerable.Empty<ClientResponseModel>(), x => x.Id == client.Id);

            var ownList = await owner.ClientApi.GetAsync<Foundation.GenericRepository.Paginations.PagedResults<System.Collections.Generic.IEnumerable<ClientResponseModel>>>(
                $"{ApiConstants.Client.ClientsApiEndpoint}?pageIndex=1&itemsPerPage=100");

            Assert.Contains(codeId, ownList.Data.Single(x => x.Id == client.Id).DiscountCodeIds);
        }

        [Fact]
        public async Task ABuyer_WithoutTheSellerRole_CannotManageCodes()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync();
            var buyerApi = await client.CreateBuyerClientApiAsync();

            var list = await buyerApi.GetForResponseAsync(ApiConstants.Client.DiscountCodesApiEndpoint);
            var read = await buyerApi.GetForResponseAsync($"{ApiConstants.Client.DiscountCodesApiEndpoint}/{codeId}");
            var create = await buyerApi.PostForResponseAsync(ApiConstants.Client.DiscountCodesApiEndpoint, new DiscountCodeRequestModel { Code = "OTHER" });
            var delete = await buyerApi.DeleteForResponseAsync($"{ApiConstants.Client.DiscountCodesApiEndpoint}/{codeId}");

            Assert.Equal(HttpStatusCode.Forbidden, list.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, read.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, create.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, delete.StatusCode);
            Assert.Single(await seller.ListCodesAsync());
        }

        // ---- Assignment through the client ----

        [Fact]
        public async Task SaveClient_WithDiscountCodeIds_AssignsThemAndDeDuplicatesRepeatedIds()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var otherCodeId = await seller.CreateCodeAsync("WINTER25");

            var client = await seller.CreateClientAsync(new[] { codeId, codeId, otherCodeId });

            var stored = await seller.GetClientAsync(client.Id);
            Assert.Equal(2, stored.DiscountCodeIds.Count());
            Assert.Contains(codeId, stored.DiscountCodeIds);
            Assert.Contains(otherCodeId, stored.DiscountCodeIds);
        }

        [Fact]
        public async Task SaveClient_WithoutDiscountCodeIds_LeavesTheAssignmentsUnchanged()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            var response = await seller.SaveClientAsync(new ClientRequestModel
            {
                Id = client.Id,
                Name = "Renamed client",
                Email = client.Email,
                CommunicationLanguage = "en",
                OrganisationId = client.OrganisationId,
                DiscountCodeIds = null
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stored = await seller.GetClientAsync(client.Id);
            Assert.Equal("Renamed client", stored.Name);
            Assert.Equal(new[] { codeId }, stored.DiscountCodeIds);
        }

        [Fact]
        public async Task SaveClient_WithAnEmptyList_ClearsTheAssignments()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            var response = await seller.SaveClientAsync(new ClientRequestModel
            {
                Id = client.Id,
                Name = "Client",
                Email = client.Email,
                CommunicationLanguage = "en",
                OrganisationId = client.OrganisationId,
                DiscountCodeIds = Array.Empty<Guid>()
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty((await seller.GetClientAsync(client.Id)).DiscountCodeIds);
            Assert.Equal("NotAssigned", (await client.ValidateAsSellerAsync(Code)).Status);
        }

        [Fact]
        public async Task SaveClient_WithACodeOfAnotherSeller_Returns422AndChangesNothing()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var ownCodeId = await seller.CreateCodeAsync(Code);
            var foreignCodeId = await other.CreateCodeAsync("FOREIGN");
            var client = await seller.CreateClientAsync(new[] { ownCodeId });

            var response = await seller.SaveClientAsync(new ClientRequestModel
            {
                Id = client.Id,
                Name = "Must not be saved",
                Email = client.Email,
                CommunicationLanguage = "en",
                OrganisationId = client.OrganisationId,
                DiscountCodeIds = new[] { ownCodeId, foreignCodeId }
            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            // No partial update: neither the name nor the assignments changed.
            var stored = await seller.GetClientAsync(client.Id);
            Assert.NotEqual("Must not be saved", stored.Name);
            Assert.Equal(new[] { ownCodeId }, stored.DiscountCodeIds);
        }

        [Fact]
        public async Task SaveClient_WithAnUnknownCodeId_Returns422()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.SaveClientAsync(new ClientRequestModel
            {
                Name = "Client",
                Email = $"buyer-{Guid.NewGuid():N}@tests.com",
                CommunicationLanguage = "en",
                OrganisationId = Guid.NewGuid(),
                DiscountCodeIds = new[] { Guid.NewGuid() }
            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SaveClient_ByANonSeller_WithAnyDiscountCodeIds_Returns403(bool emptyList)
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync();
            var buyerApi = await client.CreateBuyerClientApiAsync();

            var response = await buyerApi.PostForResponseAsync(
                ApiConstants.Client.ClientsApiEndpoint,
                new ClientRequestModel
                {
                    Id = client.Id,
                    Name = client.Email,
                    Email = client.Email,
                    CommunicationLanguage = "en",
                    OrganisationId = client.OrganisationId,
                    DiscountCodeIds = emptyList ? Array.Empty<Guid>() : new[] { codeId }
                });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("NotAssigned", (await client.ValidateAsSellerAsync(Code)).Status);
        }

        // ---- Validation ----

        [Fact]
        public async Task Validate_ForAnAssignedActiveCode_IsValidAndReturnsTheStoredSpelling()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            var answer = await client.ValidateAsSellerAsync("  summer25 ");

            Assert.Equal("Valid", answer.Status);
            Assert.Equal(Code, answer.DiscountCode);
        }

        [Fact]
        public async Task Validate_AsASeller_GivesTheSpecificReasonForEveryInapplicableCode()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var assigned = await seller.CreateCodeAsync("ASSIGNED");
            var disabled = await seller.CreateCodeAsync("DISABLED", isDisabled: true);
            await seller.CreateCodeAsync("UNASSIGNED");
            await other.CreateCodeAsync("FOREIGN");
            var client = await seller.CreateClientAsync(new[] { assigned, disabled });
            var foreignClient = await other.CreateClientAsync();

            Assert.Equal("Valid", (await client.ValidateAsSellerAsync("ASSIGNED")).Status);
            Assert.Equal("NotFound", (await client.ValidateAsSellerAsync("MISSING")).Status);
            Assert.Equal("NotFound", (await client.ValidateAsSellerAsync("FOREIGN")).Status);
            Assert.Equal("Disabled", (await client.ValidateAsSellerAsync("DISABLED")).Status);
            Assert.Equal("NotAssigned", (await client.ValidateAsSellerAsync("UNASSIGNED")).Status);

            // A client of another seller is unknown to this one, whatever the code.
            var response = await seller.ValidateAsync("ASSIGNED", foreignClient.Id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("ClientUnknown", (await ValidationAnswer.ReadAsync(response)).Status);

            var unknownClient = await seller.ValidateAsync("ASSIGNED", Guid.NewGuid());
            Assert.Equal("ClientUnknown", (await ValidationAnswer.ReadAsync(unknownClient)).Status);
        }

        [Fact]
        public async Task Validate_AsABuyer_NeverRevealsWhyACodeCannotBeApplied()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var assigned = await seller.CreateCodeAsync("ASSIGNED");
            var disabled = await seller.CreateCodeAsync("DISABLED", isDisabled: true);
            await seller.CreateCodeAsync("UNASSIGNED");
            var client = await seller.CreateClientAsync(new[] { assigned, disabled });

            var valid = await client.ValidateAsBuyerAsync("assigned");
            Assert.Equal("Valid", valid.Status);
            Assert.Equal("ASSIGNED", valid.DiscountCode);

            foreach (var code in new[] { "MISSING", "DISABLED", "UNASSIGNED" })
            {
                var answer = await client.ValidateAsBuyerAsync(code);

                Assert.Equal("NotApplicable", answer.Status);
                Assert.Null(answer.DiscountCode);
            }
        }

        [Fact]
        public async Task Validate_AsABuyer_OnlyAnswersAboutTheBuyersOwnClient()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var neighbour = await seller.CreateClientAsync(new[] { codeId });

            // The neighbour is a client of another organisation, even though it has the same seller and the same code.
            var buyerApi = await client.CreateBuyerClientApiAsync();
            var response = await buyerApi.GetForResponseAsync(DiscountSeller.ValidationUrl(Code, neighbour.Id));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var answer = await ValidationAnswer.ReadAsync(response);
            Assert.Equal("NotApplicable", answer.Status);
            Assert.Null(answer.DiscountCode);
        }

        [Fact]
        public async Task Validate_AsAClientTeamMember_IsAnsweredForTheClient()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var assigned = await seller.CreateCodeAsync("ASSIGNED");
            await seller.CreateCodeAsync("UNASSIGNED");
            var client = await seller.CreateClientAsync(new[] { assigned });
            var neighbour = await seller.CreateClientAsync(new[] { assigned });

            // The client's organisation with an email of their own: the codes are the client's, not the person's.
            var memberApi = _apiFixture.CreateClientApiClient(await client.GetTeamMemberTokenAsync());

            var valid = await ValidationAnswer.ReadAsync(await memberApi.GetForResponseAsync(DiscountSeller.ValidationUrl("assigned", client.Id)));
            Assert.Equal("Valid", valid.Status);
            Assert.Equal("ASSIGNED", valid.DiscountCode);

            var unassigned = await ValidationAnswer.ReadAsync(await memberApi.GetForResponseAsync(DiscountSeller.ValidationUrl("UNASSIGNED", client.Id)));
            Assert.Equal("NotApplicable", unassigned.Status);
            Assert.Null(unassigned.DiscountCode);

            // A team member of one client is nobody for another client.
            var foreign = await ValidationAnswer.ReadAsync(await memberApi.GetForResponseAsync(DiscountSeller.ValidationUrl("ASSIGNED", neighbour.Id)));
            Assert.Equal("NotApplicable", foreign.Status);
            Assert.Null(foreign.DiscountCode);
        }

        [Fact]
        public async Task Validate_AsABuyerOfAnotherOrganisation_IsNotApplicable()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            // Right email, wrong organisation.
            var token = await _apiFixture.GetTokenAsync(client.Email, "none", Guid.NewGuid());
            var response = await _apiFixture.CreateClientApiClient(token).GetForResponseAsync(DiscountSeller.ValidationUrl(Code, client.Id));

            Assert.Equal("NotApplicable", (await ValidationAnswer.ReadAsync(response)).Status);
        }

        [Fact]
        public async Task Validate_ForATokenWithoutAUsableOrganisation_IsClientUnknown()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            var token = await _apiFixture.GetTokenAsync(seller.Email, "Seller", Guid.Empty);
            var response = await _apiFixture.CreateClientApiClient(token).GetForResponseAsync(DiscountSeller.ValidationUrl(Code, client.Id));

            // Never a valid answer for a caller whose organisation cannot be established.
            Assert.Equal("ClientUnknown", (await ValidationAnswer.ReadAsync(response)).Status);
        }

        [Theory]
        [InlineData("", "7d4fe733-baa2-4c70-83a5-2e4ff1b5274b")]
        [InlineData("   ", "7d4fe733-baa2-4c70-83a5-2e4ff1b5274b")]
        [InlineData("SUMMER25", "00000000-0000-0000-0000-000000000000")]
        [InlineData("SUMMER25", "not-a-guid")]
        [InlineData("SUMMER25", "")]
        public async Task Validate_ForInvalidInput_Returns422(string code, string clientId)
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.ClientApi.GetForResponseAsync(
                $"{ApiConstants.Client.DiscountCodesValidationApiEndpoint}?code={Uri.EscapeDataString(code)}&clientId={Uri.EscapeDataString(clientId)}");

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Validate_ForACodeLongerThanAllowed_Returns422()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();

            var response = await seller.ValidateAsync(new string('A', 65), client.Id);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Validate_ForACodeWithSpecialCharacters_MatchesItExactly()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync("SALE&20%/ü #1");
            var client = await seller.CreateClientAsync(new[] { codeId });

            var answer = await client.ValidateAsSellerAsync("sale&20%/ü #1");

            Assert.Equal("Valid", answer.Status);
            Assert.Equal("SALE&20%/ü #1", answer.DiscountCode);
        }

        [Fact]
        public async Task Validate_RequiresAnAuthenticatedCaller()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);

            var response = await seller.ClientApi.GetForResponseAsync(DiscountSeller.ValidationUrl(Code, Guid.NewGuid()));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unauthenticated = _apiFixture.CreateClientApiClient("not-a-token");
            var rejected = await unauthenticated.GetForResponseAsync(DiscountSeller.ValidationUrl(Code, Guid.NewGuid()));

            Assert.Equal(HttpStatusCode.Unauthorized, rejected.StatusCode);
        }
    }
}
