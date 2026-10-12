using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// The discount code endpoints of Client.Api against a real SQL Server. Every test makes its own clients and
    /// codes, so none of them depends on what another test left behind.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeManagementTests
    {
        private readonly ApiFixture _apiFixture;

        public DiscountCodeManagementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private Task<DiscountCodeKit> CreateKitAsync() => DiscountCodeKit.CreateAsync(_apiFixture);

        [Fact]
        public async Task Create_WithoutClients_StoresACodeAssignedToNobody()
        {
            var kit = await CreateKitAsync();
            var codeText = DiscountCodeKit.NewCode();

            var saved = await kit.CreateCodeAsync(codeText, description: "  Summer sale  ");
            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(codeText, read.Code);
            Assert.Equal("Summer sale", read.Description);
            Assert.False(read.IsDisabled);
            Assert.Empty(read.ClientIds);
            Assert.Equal(0, read.ClientsCount);
            Assert.Equal(saved.Version, read.Version);
        }

        [Fact]
        public async Task Create_WithOneOrManyClients_AssignsExactlyThoseClients()
        {
            var kit = await CreateKitAsync();
            var first = await kit.CreateClientAsync();
            var second = await kit.CreateClientAsync();
            var third = await kit.CreateClientAsync();

            var one = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { first.Id });
            var many = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { first.Id, second.Id, third.Id });

            Assert.Equal(new[] { first.Id }, (await kit.GetAsync(one.Id)).ClientIds);
            Assert.Equal(3, (await kit.GetAsync(many.Id)).ClientsCount);
            Assert.Equal(new[] { first.Id, second.Id, third.Id }.OrderBy(x => x), (await kit.GetAsync(many.Id)).ClientIds.OrderBy(x => x));
        }

        [Fact]
        public async Task Create_TrimsTheCodeAndIgnoresARepeatedClient()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var codeText = DiscountCodeKit.NewCode();

            var saved = await kit.CreateCodeAsync($"  {codeText}  ", new[] { client.Id, client.Id });
            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(codeText, read.Code);
            Assert.Equal(new[] { client.Id }, read.ClientIds);
        }

        [Theory]
        [InlineData("same")]
        [InlineData("lower")]
        [InlineData("padded")]
        public async Task Create_WithTheTextOfAnActiveCode_IsRefusedWhateverTheLetterCasingOrSpacing(string variant)
        {
            var kit = await CreateKitAsync();
            var codeText = DiscountCodeKit.NewCode();
            await kit.CreateCodeAsync(codeText);

            var attempt = variant switch
            {
                "lower" => codeText.ToLowerInvariant(),
                "padded" => $"  {codeText}  ",
                _ => codeText
            };

            var response = await kit.SaveAsync(DiscountCodeKit.Request(attempt, Array.Empty<Guid>()));

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("A discount code with this value already exists", await RestClient.ReadMessageAsync(response));
            Assert.Single(await kit.ListAsync(codeText));
        }

        [Fact]
        public async Task Create_ConcurrentlyWithTheSameText_LetsExactlyOneSucceedAndRefusesTheRestCleanly()
        {
            var kit = await CreateKitAsync();
            var codeText = DiscountCodeKit.NewCode();

            var responses = await Task.WhenAll(Enumerable.Range(0, 8)
                .Select(_ => Task.Run(() => kit.SaveAsync(DiscountCodeKit.Request(codeText, Array.Empty<Guid>())))));

            Assert.Equal(1, responses.Count(x => x.StatusCode == HttpStatusCode.OK));
            Assert.Equal(7, responses.Count(x => x.StatusCode == HttpStatusCode.Conflict));
            Assert.Single(await kit.ListAsync(codeText));
        }

        [Fact]
        public async Task Rename_ToTheTextOfAnotherActiveCode_IsRefused_AndToItsOwnTextInOtherCasingIsAllowed()
        {
            var kit = await CreateKitAsync();
            var first = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());
            var second = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            var clash = await kit.UpdateAsync(second, Array.Empty<Guid>(), newCode: first.Code.ToLowerInvariant());

            Assert.Equal(HttpStatusCode.Conflict, clash.StatusCode);

            var ownCasing = await kit.UpdateSuccessfullyAsync(second, Array.Empty<Guid>(), newCode: second.Code.ToLowerInvariant());

            Assert.Equal(second.Code.ToLowerInvariant(), (await kit.GetAsync(ownCasing.Id)).Code);
        }

        [Fact]
        public async Task Update_RenamesDescribesAndDeactivatesTheCode()
        {
            var kit = await CreateKitAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());
            var renamed = DiscountCodeKit.NewCode();

            var response = await kit.UpdateAsync(saved, Array.Empty<Guid>(), isDisabled: true, newCode: renamed, description: "Retired");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(renamed, read.Code);
            Assert.Equal("Retired", read.Description);
            Assert.True(read.IsDisabled);
        }

        [Fact]
        public async Task Update_ReplacesTheAssignmentListAsAWhole_AndRepeatingTheSameListIsHarmless()
        {
            var kit = await CreateKitAsync();
            var first = await kit.CreateClientAsync();
            var second = await kit.CreateClientAsync();
            var third = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { first.Id, second.Id });

            var afterFirstEdit = await kit.UpdateSuccessfullyAsync(saved, new[] { second.Id, third.Id });
            var afterSecondEdit = await kit.UpdateSuccessfullyAsync(afterFirstEdit, new[] { second.Id, third.Id });
            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(new[] { second.Id, third.Id }.OrderBy(x => x), read.ClientIds.OrderBy(x => x));

            var cleared = await kit.UpdateSuccessfullyAsync(afterSecondEdit, Array.Empty<Guid>());

            Assert.Empty((await kit.GetAsync(cleared.Id)).ClientIds);
        }

        [Fact]
        public async Task Update_AnAssignmentOnlyEdit_MovesTheVersionOn()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            var updated = await kit.UpdateSuccessfullyAsync(saved, new[] { client.Id });

            Assert.NotEqual(saved.Version, updated.Version);
            Assert.Equal(updated.Version, (await kit.GetAsync(saved.Id)).Version);
        }

        [Fact]
        public async Task Update_WithAStaleVersion_IsRefusedAndChangesNothing()
        {
            var kit = await CreateKitAsync();
            var first = await kit.CreateClientAsync();
            var second = await kit.CreateClientAsync();
            var opened = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { first.Id });

            // Somebody else saves while this form is open.
            var current = await kit.UpdateSuccessfullyAsync(opened, new[] { first.Id, second.Id });

            var stale = await kit.UpdateAsync(opened, Array.Empty<Guid>(), isDisabled: true);

            Assert.Equal(HttpStatusCode.Conflict, stale.StatusCode);
            Assert.Equal("This discount code changed since you opened it. Reload before saving or deleting", await RestClient.ReadMessageAsync(stale));

            var read = await kit.GetAsync(opened.Id);

            Assert.Equal(current.Version, read.Version);
            Assert.False(read.IsDisabled);
            Assert.Equal(new[] { first.Id, second.Id }.OrderBy(x => x), read.ClientIds.OrderBy(x => x));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-base64!")]
        [InlineData("AAAA")]
        public async Task Update_WithoutAUsableVersion_IsRefused(string version)
        {
            var kit = await CreateKitAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            var response = await kit.SaveAsync(DiscountCodeKit.Request(saved.Code, Array.Empty<Guid>(), id: saved.Id, version: version));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithAClientOfAnotherSeller_IsRefusedBeforeAnythingChanges()
        {
            var kit = await CreateKitAsync();
            var own = await kit.CreateClientAsync();
            var foreign = await kit.CreateClientAsync(kit.OtherSellerApi);
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { own.Id });

            var response = await kit.UpdateAsync(saved, new[] { own.Id, foreign.Id }, isDisabled: true, newCode: DiscountCodeKit.NewCode());

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("One or more selected clients are unavailable for assignment", await RestClient.ReadMessageAsync(response));

            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(saved.Code, read.Code);
            Assert.False(read.IsDisabled);
            Assert.Equal(saved.Version, read.Version);
            Assert.Equal(new[] { own.Id }, read.ClientIds);
        }

        [Fact]
        public async Task Create_WithAnUnknownOrDeletedOrForeignClient_IsRefusedAndCreatesNothing()
        {
            var kit = await CreateKitAsync();
            var deleted = await kit.CreateClientAsync();
            var foreign = await kit.CreateClientAsync(kit.OtherSellerApi);
            Assert.Equal(HttpStatusCode.OK, (await kit.DeleteClientAsync(deleted)).StatusCode);

            foreach (var clientId in new[] { Guid.NewGuid(), deleted.Id, foreign.Id })
            {
                var codeText = DiscountCodeKit.NewCode();

                var response = await kit.SaveAsync(DiscountCodeKit.Request(codeText, new[] { clientId }));

                Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
                Assert.Empty(await kit.ListAsync(codeText));
            }
        }

        [Fact]
        public async Task Create_WithAnEmptyClientId_IsRefusedAsMalformed()
        {
            var kit = await CreateKitAsync();

            var response = await kit.SaveAsync(DiscountCodeKit.Request(DiscountCodeKit.NewCode(), new[] { Guid.Empty }));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Create_WithABlankCode_IsRefused(string code)
        {
            var kit = await CreateKitAsync();

            var response = await kit.SaveAsync(DiscountCodeKit.Request(code, Array.Empty<Guid>()));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Create_WithACodeOrDescriptionThatIsTooLong_IsRefused_AndTheLimitsThemselvesAreAccepted()
        {
            var kit = await CreateKitAsync();

            var tooLongCode = await kit.SaveAsync(DiscountCodeKit.Request(new string('A', 65), Array.Empty<Guid>()));
            var tooLongDescription = await kit.SaveAsync(DiscountCodeKit.Request(DiscountCodeKit.NewCode(), Array.Empty<Guid>(), description: new string('d', 257)));
            var atTheLimits = await kit.SaveAsync(DiscountCodeKit.Request($"L{Guid.NewGuid():N}{new string('X', 31)}", Array.Empty<Guid>(), description: new string('d', 256)));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, tooLongCode.StatusCode);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, tooLongDescription.StatusCode);
            Assert.Equal(HttpStatusCode.OK, atTheLimits.StatusCode);
        }

        [Fact]
        public async Task Delete_RemovesTheCodeAndItsAssignments_AndLetsTheTextBeUsedAgain()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { client.Id });

            var deleted = await kit.DeleteAsync(saved.Id, saved.Version);

            Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await kit.GetForResponseAsync(saved.Id)).StatusCode);
            Assert.Empty(await kit.ListAsync(saved.Code));
            Assert.Equal("NotFound", (await kit.ValidateOkAsync(kit.SellerApi, saved.Code, client.Id)).Status);

            var recreated = await kit.CreateCodeAsync(saved.Code);

            Assert.NotEqual(saved.Id, recreated.Id);
            Assert.Empty((await kit.GetAsync(recreated.Id)).ClientIds);
        }

        [Fact]
        public async Task Delete_WithAStaleVersion_IsRefusedAndKeepsTheCode()
        {
            var kit = await CreateKitAsync();
            var opened = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());
            var current = await kit.UpdateSuccessfullyAsync(opened, Array.Empty<Guid>(), isDisabled: true);

            var stale = await kit.DeleteAsync(opened.Id, opened.Version);

            Assert.Equal(HttpStatusCode.Conflict, stale.StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await kit.GetForResponseAsync(opened.Id)).StatusCode);

            Assert.Equal(HttpStatusCode.OK, (await kit.DeleteAsync(opened.Id, current.Version)).StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("not-base64!")]
        public async Task Delete_WithoutAUsableVersion_IsRefused(string version)
        {
            var kit = await CreateKitAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            var response = await kit.DeleteAsync(saved.Id, version);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await kit.GetForResponseAsync(saved.Id)).StatusCode);
        }

        [Fact]
        public async Task DeletingAClient_RemovesItFromEveryCodeAssignedToIt()
        {
            var kit = await CreateKitAsync();
            var keptClient = await kit.CreateClientAsync();
            var deletedClient = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { keptClient.Id, deletedClient.Id });

            Assert.Equal(HttpStatusCode.OK, (await kit.DeleteClientAsync(deletedClient)).StatusCode);

            var read = await kit.GetAsync(saved.Id);

            Assert.Equal(new[] { keptClient.Id }, read.ClientIds);
            Assert.Equal(1, read.ClientsCount);
            Assert.Equal("ClientUnknown", (await kit.ValidateOkAsync(kit.SellerApi, saved.Code, deletedClient.Id)).Status);
        }

        [Fact]
        public async Task TheList_ShowsOnlyTheSellersOwnCodes_WithTheirClientCounts_AndCanBeSearched()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var prefix = $"LS{Guid.NewGuid():N}".Substring(0, 12).ToUpperInvariant();
            var withClient = await kit.CreateCodeAsync($"{prefix}A", new[] { client.Id });
            var without = await kit.CreateCodeAsync($"{prefix}B");
            await kit.CreateCodeAsync($"{prefix}C", api: kit.OtherSellerApi);

            var own = await kit.ListAsync(prefix);
            var other = await kit.ListAsync(prefix, kit.OtherSellerApi);

            Assert.Equal(new[] { withClient.Id, without.Id }.OrderBy(x => x), own.Select(x => x.Id.Value).OrderBy(x => x));
            Assert.Equal(1, own.Single(x => x.Id == withClient.Id).ClientsCount);
            Assert.Equal(0, own.Single(x => x.Id == without.Id).ClientsCount);
            Assert.All(own, x => Assert.False(string.IsNullOrWhiteSpace(x.Version)));
            Assert.Single(other);
            Assert.Empty(await kit.ListAsync($"{prefix}Z"));
        }

        [Fact]
        public async Task AnotherSeller_CannotReadChangeDeleteOrAssignTheFirstSellersData()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { client.Id });

            Assert.Equal(HttpStatusCode.NotFound, (await kit.GetForResponseAsync(saved.Id, kit.OtherSellerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await kit.UpdateAsync(saved, Array.Empty<Guid>(), isDisabled: true, api: kit.OtherSellerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await kit.DeleteAsync(saved.Id, saved.Version, kit.OtherSellerApi)).StatusCode);
            Assert.Empty(await kit.ListAsync(saved.Code, kit.OtherSellerApi));

            var untouched = await kit.GetAsync(saved.Id);

            Assert.False(untouched.IsDisabled);
            Assert.Equal(saved.Version, untouched.Version);
            Assert.Equal(new[] { client.Id }, untouched.ClientIds);

            // The other seller cannot assign the first seller's client to a code of their own either.
            var response = await kit.SaveAsync(DiscountCodeKit.Request(DiscountCodeKit.NewCode(), new[] { client.Id }), kit.OtherSellerApi);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task TwoSellers_CanEachHaveACodeWithTheSameText()
        {
            var kit = await CreateKitAsync();
            var codeText = DiscountCodeKit.NewCode();

            var first = await kit.CreateCodeAsync(codeText);
            var second = await kit.CreateCodeAsync(codeText, api: kit.OtherSellerApi);

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public async Task TheClientOptions_AreTheSellersOwnActiveClients_IncludingDisabledOnes_AndNeverAnotherSellersOrDeletedOnes()
        {
            var kit = await CreateKitAsync();
            var marker = $"Opt{Guid.NewGuid():N}".Substring(0, 12);
            var active = await kit.CreateClientAsync(namePrefix: $"{marker} active");
            var disabled = await kit.CreateClientAsync(namePrefix: $"{marker} disabled");
            var deleted = await kit.CreateClientAsync(namePrefix: $"{marker} deleted");
            var foreign = await kit.CreateClientAsync(kit.OtherSellerApi, $"{marker} foreign");

            var disable = await kit.SellerApi.PostForResponseAsync(Definitions.ApiEndpoints.ClientApiClientsEndpoint, new Client.Api.v1.RequestModels.ClientRequestModel
            {
                Id = disabled.Id,
                Name = disabled.Name,
                Email = disabled.Email,
                CommunicationLanguage = Definitions.Clients.Language,
                OrganisationId = disabled.OrganisationId,
                IsDisabled = true
            });
            Assert.Equal(HttpStatusCode.OK, disable.StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await kit.DeleteClientAsync(deleted)).StatusCode);

            var options = await kit.GetOptionsAsync(marker);
            var otherSellerOptions = await kit.GetOptionsAsync(marker, kit.OtherSellerApi);

            Assert.Equal(new[] { active.Id, disabled.Id }.OrderBy(x => x), options.Select(x => x.Id.Value).OrderBy(x => x));
            Assert.Equal(new[] { foreign.Id }, otherSellerOptions.Select(x => x.Id.Value));
            Assert.Equal(active.Name, options.Single(x => x.Id == active.Id).Name);
        }

        [Fact]
        public async Task TheClientOptions_AreOrderedByNameAndPaged()
        {
            var kit = await CreateKitAsync();
            var marker = $"Pg{Guid.NewGuid():N}".Substring(0, 12);

            for (var i = 3; i >= 1; i--)
            {
                await kit.CreateClientAsync(namePrefix: $"{marker} {i}");
            }

            var firstPage = await kit.SellerApi.GetAsync<Foundation.GenericRepository.Paginations.PagedResults<IEnumerable<Client.Api.v1.ResponseModels.DiscountCodeClientOptionResponseModel>>>(
                $"{Definitions.ApiEndpoints.ClientApiDiscountCodeClientsEndpoint}?searchTerm={marker}&pageIndex=1&itemsPerPage=2&orderBy={Uri.EscapeDataString("Name asc")}");

            Assert.Equal(3, firstPage.Total);
            Assert.Equal(2, firstPage.Data.Count());
            Assert.True(firstPage.Data.First().Name.CompareTo(firstPage.Data.Last().Name) < 0);
        }

        [Fact]
        public async Task SomebodyWhoIsNoSeller_IsForbiddenFromManagingCodesAndListingOptions()
        {
            var kit = await CreateKitAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            Assert.Equal(HttpStatusCode.Forbidden, (await kit.BuyerApi.GetForResponseAsync(Definitions.ApiEndpoints.ClientApiDiscountCodesEndpoint)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await kit.BuyerApi.GetForResponseAsync(Definitions.ApiEndpoints.ClientApiDiscountCodeClientsEndpoint)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await kit.GetForResponseAsync(saved.Id, kit.BuyerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await kit.SaveAsync(DiscountCodeKit.Request(DiscountCodeKit.NewCode(), Array.Empty<Guid>()), kit.BuyerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await kit.UpdateAsync(saved, Array.Empty<Guid>(), api: kit.BuyerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await kit.DeleteAsync(saved.Id, saved.Version, kit.BuyerApi)).StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await kit.GetForResponseAsync(saved.Id)).StatusCode);
        }

        [Fact]
        public async Task Validation_ForASeller_NamesTheReasonAnApplicationWouldFail()
        {
            var kit = await CreateKitAsync();
            var assigned = await kit.CreateClientAsync();
            var other = await kit.CreateClientAsync();
            var codeText = DiscountCodeKit.NewCode();
            var inactive = DiscountCodeKit.NewCode();
            await kit.CreateCodeAsync(codeText, new[] { assigned.Id });
            await kit.CreateCodeAsync(inactive, new[] { assigned.Id }, isDisabled: true);

            Assert.Equal(("Valid", codeText), await kit.ValidateOkAsync(kit.SellerApi, codeText.ToLowerInvariant(), assigned.Id));
            Assert.Equal(("Valid", codeText), await kit.ValidateOkAsync(kit.SellerApi, $"  {codeText}  ", assigned.Id));
            Assert.Equal(("NotAssigned", null), await kit.ValidateOkAsync(kit.SellerApi, codeText, other.Id));
            Assert.Equal(("Disabled", null), await kit.ValidateOkAsync(kit.SellerApi, inactive, assigned.Id));
            Assert.Equal(("NotFound", null), await kit.ValidateOkAsync(kit.SellerApi, DiscountCodeKit.NewCode(), assigned.Id));
            Assert.Equal(("ClientUnknown", null), await kit.ValidateOkAsync(kit.SellerApi, codeText, Guid.NewGuid()));
        }

        [Fact]
        public async Task Validation_ForACodeAssignedToNobody_NeverAppliesToAnyone()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode());

            Assert.Equal(("NotAssigned", null), await kit.ValidateOkAsync(kit.SellerApi, saved.Code, client.Id));
        }

        [Fact]
        public async Task Validation_ForASellerAskingAboutAnotherSellersClient_ReportsAnUnknownClient()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var saved = await kit.CreateCodeAsync(DiscountCodeKit.NewCode(), new[] { client.Id });

            Assert.Equal(("ClientUnknown", null), await kit.ValidateOkAsync(kit.OtherSellerApi, saved.Code, client.Id));
        }

        [Fact]
        public async Task Validation_ForABuyerAndTheirTeam_AcceptsTheirClientsCodesAndNeverSaysWhyAnotherIsRefused()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();
            var otherClient = await kit.CreateClientAsync();
            var assigned = DiscountCodeKit.NewCode();
            var notAssigned = DiscountCodeKit.NewCode();
            var inactive = DiscountCodeKit.NewCode();
            await kit.CreateCodeAsync(assigned, new[] { client.Id });
            await kit.CreateCodeAsync(notAssigned, new[] { otherClient.Id });
            await kit.CreateCodeAsync(inactive, new[] { client.Id }, isDisabled: true);

            var buyerToken = await _apiFixture.IssueBuyerTokenAsync(client.Email, client.OrganisationId);
            var teamMemberToken = await _apiFixture.IssueBuyerTokenAsync($"team.{Guid.NewGuid():N}@dc.test", client.OrganisationId);
            var strangerToken = await _apiFixture.IssueBuyerTokenAsync($"stranger.{Guid.NewGuid():N}@dc.test", otherClient.OrganisationId);

            foreach (var token in new[] { buyerToken, teamMemberToken })
            {
                var api = _apiFixture.CreateClientApiClient(token);

                Assert.Equal(("Valid", assigned), await kit.ValidateOkAsync(api, assigned.ToLowerInvariant(), client.Id));
                Assert.Equal(("NotApplicable", null), await kit.ValidateOkAsync(api, notAssigned, client.Id));
                Assert.Equal(("NotApplicable", null), await kit.ValidateOkAsync(api, inactive, client.Id));
                Assert.Equal(("NotApplicable", null), await kit.ValidateOkAsync(api, DiscountCodeKit.NewCode(), client.Id));
            }

            // Somebody of another client's organisation cannot ask about this client at all.
            var stranger = _apiFixture.CreateClientApiClient(strangerToken);

            Assert.Equal(("NotApplicable", null), await kit.ValidateOkAsync(stranger, assigned, client.Id));
        }

        [Fact]
        public async Task Validation_WithMalformedInput_IsRefusedAsUnprocessable()
        {
            var kit = await CreateKitAsync();
            var client = await kit.CreateClientAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, (await kit.ValidateAsync(kit.SellerApi, "  ", client.Id)).StatusCode);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, (await kit.ValidateAsync(kit.SellerApi, new string('A', 65), client.Id)).StatusCode);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, (await kit.ValidateAsync(kit.SellerApi, "CODE", Guid.Empty)).StatusCode);
            Assert.Equal(
                HttpStatusCode.UnprocessableEntity,
                (await kit.SellerApi.GetForResponseAsync($"{Definitions.ApiEndpoints.ClientApiDiscountCodeValidationEndpoint}?code=CODE&clientId=not-a-guid")).StatusCode);
            Assert.Equal(
                HttpStatusCode.UnprocessableEntity,
                (await kit.SellerApi.GetForResponseAsync($"{Definitions.ApiEndpoints.ClientApiDiscountCodeValidationEndpoint}?clientId={client.Id}")).StatusCode);
        }
    }
}
