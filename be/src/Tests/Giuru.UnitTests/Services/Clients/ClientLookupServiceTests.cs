using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Threading.Tasks;
using ClientLookupService = Seller.Web.Shared.Services.Clients.ClientLookupService;
using IClientsRepository = Seller.Web.Shared.Repositories.Clients.IClientsRepository;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;

namespace Giuru.UnitTests.Services.Clients
{
    // The service is registered scoped so that the price-client resolver and the callers that also
    // need the client's organisation share one fetch per request instead of each issuing their own.
    // These tests pin that memoisation, because losing it silently doubles an outbound call.
    public class ClientLookupServiceTests
    {
        [Fact]
        public async Task GetAsync_WhenCalledRepeatedlyForTheSameClient_FetchesOnce()
        {
            var clientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", clientId)
                .Returns(Task.FromResult(new SellerClient { Id = clientId }));
            var service = CreateService(clientsRepository);

            var first = await service.GetAsync("token", clientId);
            var second = await service.GetAsync("token", clientId);

            Assert.Same(first, second);
            await clientsRepository.Received(1).GetClientAsync("token", "en-US", clientId);
        }

        [Fact]
        public async Task GetAsync_WhenTheClientChanges_FetchesAgain()
        {
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", firstClientId)
                .Returns(Task.FromResult(new SellerClient { Id = firstClientId }));
            clientsRepository.GetClientAsync("token", "en-US", secondClientId)
                .Returns(Task.FromResult(new SellerClient { Id = secondClientId }));
            var service = CreateService(clientsRepository);

            var first = await service.GetAsync("token", firstClientId);
            var second = await service.GetAsync("token", secondClientId);

            Assert.Equal(firstClientId, first.Id);
            Assert.Equal(secondClientId, second.Id);
            await clientsRepository.Received(1).GetClientAsync("token", "en-US", firstClientId);
            await clientsRepository.Received(1).GetClientAsync("token", "en-US", secondClientId);
        }

        [Fact]
        public async Task GetAsync_ReadsWithTheConfiguredDefaultCultureRatherThanTheRequestCulture()
        {
            var clientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(Task.FromResult(new SellerClient { Id = clientId }));
            var service = CreateService(clientsRepository, defaultCulture: "de-DE");

            await service.GetAsync("token", clientId);

            await clientsRepository.Received(1).GetClientAsync("token", "de-DE", clientId);
        }

        [Fact]
        public async Task GetAsync_WhenNoClientIdIsSupplied_StillMemoisesTheLookup()
        {
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", null)
                .Returns(Task.FromResult<SellerClient>(null));
            var service = CreateService(clientsRepository);

            Assert.Null(await service.GetAsync("token", null));
            Assert.Null(await service.GetAsync("token", null));

            await clientsRepository.Received(1).GetClientAsync("token", "en-US", null);
        }

        private static ClientLookupService CreateService(IClientsRepository clientsRepository, string defaultCulture = "en-US")
        {
            return new ClientLookupService(
                clientsRepository,
                Options.Create(new SellerAppSettings { DefaultCulture = defaultCulture }));
        }
    }
}
