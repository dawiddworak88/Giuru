using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using ClientLookupService = Seller.Web.Shared.Services.Clients.ClientLookupService;
using IClientsRepository = Seller.Web.Shared.Repositories.Clients.IClientsRepository;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;

namespace Giuru.UnitTests.Services.Clients
{
    // The service is registered scoped so that the price-client resolver and the callers that also
    // need the client's organisation share one fetch per request instead of each issuing their own.
    // These tests pin that memoisation - keyed on both the token and the client id, so a caller
    // presenting a different token re-reads rather than inheriting another token's client - and pin
    // that concurrent callers for the same key share a single in-flight fetch rather than racing.
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

        [Fact]
        public async Task GetAsync_WhenTheTokenChangesForTheSameClient_FetchesAgain()
        {
            var clientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("first-token", "en-US", clientId)
                .Returns(Task.FromResult(new SellerClient { Id = clientId, Name = "first" }));
            clientsRepository.GetClientAsync("second-token", "en-US", clientId)
                .Returns(Task.FromResult(new SellerClient { Id = clientId, Name = "second" }));
            var service = CreateService(clientsRepository);

            var first = await service.GetAsync("first-token", clientId);
            var second = await service.GetAsync("second-token", clientId);

            Assert.Equal("first", first.Name);
            Assert.Equal("second", second.Name);
            await clientsRepository.Received(1).GetClientAsync("first-token", "en-US", clientId);
            await clientsRepository.Received(1).GetClientAsync("second-token", "en-US", clientId);
        }

        // Probabilistic against the old implementation, deterministic against the gated one: it pins
        // that every concurrent caller receives the very same task, which is what the comment claims.
        [Fact]
        public async Task GetAsync_WhenCalledConcurrentlyForTheSameClient_FetchesOnce()
        {
            var clientId = Guid.NewGuid();
            var release = new TaskCompletionSource<SellerClient>(TaskCreationOptions.RunContinuationsAsynchronously);
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", clientId).Returns(release.Task);
            var service = CreateService(clientsRepository);

            var calls = await Task.WhenAll(
                Enumerable.Range(0, 8).Select(_ => Task.Run<Task<SellerClient>>(() => service.GetAsync("token", clientId))));

            release.SetResult(new SellerClient { Id = clientId });
            await Task.WhenAll(calls);

            Assert.All(calls, task => Assert.Same(calls[0], task));
            await clientsRepository.Received(1).GetClientAsync("token", "en-US", clientId);
        }

        // A faulted read must not be memoised. Sharing one fetch is a request-scoped optimisation;
        // sharing one *failure* would turn a single transient repository error into a failure of
        // every later consumer in the request - on the seller basket-save path, of the whole order.
        [Fact]
        public async Task GetAsync_WhenTheReadFails_DoesNotMemoiseTheFailureAndLetsTheNextCallerRetry()
        {
            var clientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", clientId)
                .Returns(
                    _ => Task.FromException<SellerClient>(new InvalidOperationException("transient")),
                    _ => Task.FromResult(new SellerClient { Id = clientId, Name = "recovered" }));
            var service = CreateService(clientsRepository);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAsync("token", clientId));

            var retried = await service.GetAsync("token", clientId);

            Assert.Equal("recovered", retried.Name);
            await clientsRepository.Received(2).GetClientAsync("token", "en-US", clientId);
        }

        [Fact]
        public async Task GetAsync_AfterARecoveredRead_MemoisesTheSuccessfulResult()
        {
            var clientId = Guid.NewGuid();
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync("token", "en-US", clientId)
                .Returns(
                    _ => Task.FromException<SellerClient>(new InvalidOperationException("transient")),
                    _ => Task.FromResult(new SellerClient { Id = clientId, Name = "recovered" }));
            var service = CreateService(clientsRepository);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetAsync("token", clientId));

            var first = await service.GetAsync("token", clientId);
            var second = await service.GetAsync("token", clientId);

            Assert.Same(first, second);
            await clientsRepository.Received(2).GetClientAsync("token", "en-US", clientId);
        }

        private static ClientLookupService CreateService(IClientsRepository clientsRepository, string defaultCulture = "en-US")
        {
            return new ClientLookupService(
                clientsRepository,
                Options.Create(new SellerAppSettings { DefaultCulture = defaultCulture }));
        }
    }
}
