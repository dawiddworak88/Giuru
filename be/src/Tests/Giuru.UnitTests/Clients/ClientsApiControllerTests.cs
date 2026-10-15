using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Services.Claims;
using Foundation.Localization;
using Giuru.UnitTests.Helpers;
using Giuru.UnitTests.Orders.Baskets;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Seller.Web.Areas.Clients.ApiControllers;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Shared.DomainModels.Organisations;
using Seller.Web.Shared.Repositories.Organisations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerIClientsRepository = Seller.Web.Shared.Repositories.Clients.IClientsRepository;

namespace Giuru.UnitTests.Clients
{
    // The buyer app caches a client's claims per organisation for 30 minutes. Every change that alters which client an
    // organisation prices for must renew that organisation's version, or its members keep pricing - and checking out -
    // as the client they no longer belong to.
    public class ClientsApiControllerTests
    {
        private const string Email = "client@test.com";

        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid OrganisationId = Guid.NewGuid();
        private static readonly Guid OtherOrganisationId = Guid.NewGuid();

        private readonly IOrganisationsRepository _organisationsRepository = Substitute.For<IOrganisationsRepository>();
        private readonly SellerIClientsRepository _clientsRepository = Substitute.For<SellerIClientsRepository>();
        private readonly IClaimsCacheInvalidatorService _cacheInvalidatorService = Substitute.For<IClaimsCacheInvalidatorService>();

        private ClientsApiController CreateController()
        {
            return new ClientsApiController(
                _organisationsRepository,
                _clientsRepository,
                TestLocalizer.Create<ClientResources>(),
                Substitute.For<IClientGroupsRepository>(),
                Substitute.For<IClientFieldValuesRepository>(),
                _cacheInvalidatorService)
            {
                ControllerContext = new ControllerContext { HttpContext = SellerFlowHelpers.CreateHttpContext() }
            };
        }

        private void GivenTheClientIsIn(Guid organisationId)
        {
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>(), ClientId)
                .Returns(Task.FromResult(new SellerClient { Id = ClientId, OrganisationId = organisationId }));
        }

        private void GivenTheEmailBelongsTo(Guid organisationId)
        {
            _organisationsRepository.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new Organisation { Id = organisationId, Name = "Client" }));
        }

        private static SaveClientRequestModel SaveRequest(Guid? id)
        {
            return new SaveClientRequestModel { Id = id, Name = "Client", Email = Email };
        }

        [Fact]
        public async Task Delete_InvalidatesTheClaimsOfTheClientsOrganisationOnceTheClientIsDeleted()
        {
            GivenTheClientIsIn(OrganisationId);

            await CreateController().Delete(ClientId);

            Received.InOrder(() =>
            {
                _clientsRepository.DeleteAsync(Arg.Any<string>(), Arg.Any<string>(), ClientId);
                _cacheInvalidatorService.InvalidateAsync(OrganisationId);
            });
        }

        [Fact]
        public async Task Index_WhenANewEmailMovesTheClientToAnotherOrganisation_InvalidatesTheOrganisationItLeavesAsWell()
        {
            GivenTheClientIsIn(OrganisationId);
            GivenTheEmailBelongsTo(OtherOrganisationId);

            await CreateController().Index(SaveRequest(ClientId));

            await _cacheInvalidatorService.Received(1).InvalidateAsync(OtherOrganisationId);
            await _cacheInvalidatorService.Received(1).InvalidateAsync(OrganisationId);
        }

        [Fact]
        public async Task Index_WhenTheClientStaysInItsOrganisation_InvalidatesItOnce()
        {
            GivenTheClientIsIn(OrganisationId);
            GivenTheEmailBelongsTo(OrganisationId);

            await CreateController().Index(SaveRequest(ClientId));

            await _cacheInvalidatorService.Received(1).InvalidateAsync(Arg.Any<Guid?>());
            await _cacheInvalidatorService.Received(1).InvalidateAsync(OrganisationId);
        }

        // An email that is another client's finds that client's organisation; Client.Api refuses the save, and the other
        // client's organisation must not have been renamed after the client being edited by then.
        [Fact]
        public async Task Index_WhenTheSaveIsRefused_DoesNotRenameTheOrganisationOfTheEmail()
        {
            GivenTheClientIsIn(OrganisationId);
            GivenTheEmailBelongsTo(OtherOrganisationId);
            _clientsRepository.SaveAsync(default, default, default, default, default, default, default, default, default, default, default, default, default, default, default)
                .ReturnsForAnyArgs<Task<Guid>>(_ => throw new CustomException("ClientExists", (int)HttpStatusCode.Conflict));

            var request = SaveRequest(ClientId);
            request.Name = "Another name";

            await Assert.ThrowsAsync<CustomException>(() => CreateController().Index(request));

            await _organisationsRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default, default, default, default);
        }

        [Fact]
        public async Task Index_WhenTheNameChanges_RenamesTheOrganisationOnceTheClientIsSaved()
        {
            GivenTheClientIsIn(OrganisationId);
            GivenTheEmailBelongsTo(OrganisationId);

            var request = SaveRequest(ClientId);
            request.Name = "Another name";

            await CreateController().Index(request);

            Received.InOrder(() =>
            {
                _clientsRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), ClientId, "Another name", Email, Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<bool>(), OrganisationId, Arg.Any<IEnumerable<Guid>>(), Arg.Any<IEnumerable<Guid>>(), Arg.Any<Guid?>(), Arg.Any<Guid?>());
                _organisationsRepository.UpdateAsync(Arg.Any<string>(), Arg.Any<string>(), OrganisationId, "Another name");
            });
        }

        [Fact]
        public async Task Index_ForANewClient_InvalidatesOnlyTheOrganisationOfItsEmail()
        {
            GivenTheEmailBelongsTo(OrganisationId);

            await CreateController().Index(SaveRequest(null));

            await _clientsRepository.DidNotReceiveWithAnyArgs().GetClientAsync(default, default, default);
            await _cacheInvalidatorService.Received(1).InvalidateAsync(Arg.Any<Guid?>());
            await _cacheInvalidatorService.Received(1).InvalidateAsync(OrganisationId);
        }
    }
}
