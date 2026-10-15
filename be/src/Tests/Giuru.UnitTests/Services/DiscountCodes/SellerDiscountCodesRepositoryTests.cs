using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Giuru.UnitTests.Helpers;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class SellerDiscountCodesRepositoryTests
    {
        private static DiscountCodesRepository CreateRepository(string clientUrl)
        {
            return new DiscountCodesRepository(new ApiClientService(), Options.Create(new SellerAppSettings { ClientUrl = clientUrl }));
        }

        [Fact]
        public async Task GetClientOptionsAsync_FollowsEveryPageInAStableOrder()
        {
            var clients = Enumerable.Range(1, 120).Select(i => new { id = Guid.NewGuid(), name = $"Client {i:000}" }).ToList();
            await using var api = await FakeClientApi.StartAsync(request =>
            {
                var pageIndex = int.Parse(request.Query["PageIndex"].ToString());
                var itemsPerPage = int.Parse(request.Query["ItemsPerPage"].ToString());

                return (200, JsonSerializer.Serialize(new
                {
                    data = clients.Skip((pageIndex - 1) * itemsPerPage).Take(itemsPerPage),
                    total = clients.Count,
                    pageSize = itemsPerPage
                }));
            });

            var options = (await CreateRepository(api.Url).GetClientOptionsAsync("token", "en")).ToList();

            Assert.Equal(clients.Select(x => x.id), options.Select(x => x.Id));
            Assert.Equal(new[] { "1", "2", "3" }, api.Requests.Select(x => x.Query["PageIndex"].ToString()));
            Assert.All(api.Requests, request =>
            {
                Assert.Equal("/api/v1/discountcodes/clients", request.Path);
                Assert.Equal("Name asc", request.Query["OrderBy"].ToString());
            });
        }

        [Fact]
        public async Task GetClientOptionsAsync_ForASellerWithoutClients_ReturnsAnEmptyList()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"data\":[],\"total\":0,\"pageSize\":50}"));

            var options = await CreateRepository(api.Url).GetClientOptionsAsync("token", "en");

            Assert.Empty(options);
        }

        [Fact]
        public async Task GetClientOptionsAsync_WhenAPageFails_ThrowsInsteadOfReturningAShorterList()
        {
            var clients = Enumerable.Range(1, 120).Select(i => new { id = Guid.NewGuid(), name = $"Client {i:000}" }).ToList();
            await using var api = await FakeClientApi.StartAsync(request =>
            {
                var pageIndex = int.Parse(request.Query["PageIndex"].ToString());

                return pageIndex == 2
                    ? (500, "{\"message\":\"boom\"}")
                    : (200, JsonSerializer.Serialize(new { data = clients.Take(50), total = clients.Count, pageSize = 50 }));
            });

            await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).GetClientOptionsAsync("token", "en"));
        }

        [Fact]
        public async Task GetClientOptionsAsync_WhenTheListIsShorterThanTheTotalSaysItIs_Throws()
        {
            var clients = Enumerable.Range(1, 60).Select(i => new { id = Guid.NewGuid(), name = $"Client {i:000}" }).ToList();
            await using var api = await FakeClientApi.StartAsync(request =>
            {
                var pageIndex = int.Parse(request.Query["PageIndex"].ToString());

                // The second page comes back shorter than the total promised.
                return (200, JsonSerializer.Serialize(new
                {
                    data = clients.Skip((pageIndex - 1) * 50).Take(pageIndex == 2 ? 5 : 50),
                    total = clients.Count,
                    pageSize = 50
                }));
            });

            await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).GetClientOptionsAsync("token", "en"));
        }

        [Fact]
        public async Task GetClientOptionsAsync_WhenAClientIsRepeatedAcrossPages_Throws()
        {
            var repeated = new { id = Guid.NewGuid(), name = "Repeated" };
            await using var api = await FakeClientApi.StartAsync(request =>
            {
                var pageIndex = int.Parse(request.Query["PageIndex"].ToString());

                return (200, JsonSerializer.Serialize(new
                {
                    data = pageIndex == 1
                        ? new[] { repeated }.Concat(Enumerable.Range(1, 49).Select(i => new { id = Guid.NewGuid(), name = $"C{i}" }))
                        : new[] { repeated },
                    total = 51,
                    pageSize = 50
                }));
            });

            await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).GetClientOptionsAsync("token", "en"));
        }

        [Fact]
        public async Task GetClientOptionsAsync_WhenTheApiRefusesTheSeller_Throws()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (403, null));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).GetClientOptionsAsync("token", "en"));

            Assert.Equal((int)HttpStatusCode.Forbidden, exception.StatusCode);
        }

        [Fact]
        public async Task SaveAsync_AlwaysSendsTheAssignmentListEvenWhenEmptyAndReadsTheNewVersion()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"id\":\"5b0a2d0f-71a3-4d3a-a7b1-2c28a31b6a01\",\"version\":\"AAAAAAAAB9E=\"}"));

            var saved = await CreateRepository(api.Url).SaveAsync("token", "en", null, null, "SUMMER25", null, false, null);

            Assert.Equal(Guid.Parse("5b0a2d0f-71a3-4d3a-a7b1-2c28a31b6a01"), saved.Id);
            Assert.Equal("AAAAAAAAB9E=", saved.Version);

            var request = Assert.Single(api.Requests);
            Assert.Equal("POST", request.Method);

            using var body = JsonDocument.Parse(request.Body);
            Assert.Equal(JsonValueKind.Array, body.RootElement.GetProperty("clientIds").ValueKind);
            Assert.Equal(0, body.RootElement.GetProperty("clientIds").GetArrayLength());
            Assert.Equal("SUMMER25", body.RootElement.GetProperty("code").GetString());
        }

        [Fact]
        public async Task SaveAsync_SendsTheIdVersionAndAssignedClientsOfAnUpdate()
        {
            var id = Guid.NewGuid();
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            await using var api = await FakeClientApi.StartAsync(_ => (200, $"{{\"id\":\"{id}\",\"version\":\"AAAAAAAAB9I=\"}}"));

            await CreateRepository(api.Url).SaveAsync("token", "en", id, "AAAAAAAAB9E=", "SUMMER25", "Summer", true, new[] { firstClientId, secondClientId });

            using var body = JsonDocument.Parse(Assert.Single(api.Requests).Body);
            Assert.Equal(id, body.RootElement.GetProperty("id").GetGuid());
            Assert.Equal("AAAAAAAAB9E=", body.RootElement.GetProperty("version").GetString());
            Assert.True(body.RootElement.GetProperty("isDisabled").GetBoolean());
            Assert.Equal(
                new[] { firstClientId, secondClientId },
                body.RootElement.GetProperty("clientIds").EnumerateArray().Select(x => x.GetGuid()));
        }

        [Fact]
        public async Task SaveAsync_WhenTheApiReportsAConflict_ThrowsWithItsStatusAndMessage()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (409, "{\"message\":\"changed\"}"));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).SaveAsync("token", "en", Guid.NewGuid(), "AAAAAAAAB9E=", "SUMMER25", null, false, new Guid[0]));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            Assert.Equal("changed", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_SendsTheVersionEncodedInTheQuery()
        {
            var id = Guid.NewGuid();
            await using var api = await FakeClientApi.StartAsync(_ => (200, null));
            const string version = "AB+/cd==";

            await CreateRepository(api.Url).DeleteAsync("token", "en", id, version);

            var request = Assert.Single(api.Requests);
            Assert.Equal("DELETE", request.Method);
            Assert.Equal($"/api/v1/discountcodes/{id}", request.Path);
            Assert.Equal(version, request.Query["Version"].ToString());
        }

        [Fact]
        public async Task DeleteAsync_WhenTheApiReportsAConflict_ThrowsSoTheRowIsKept()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (409, "{\"message\":\"changed\"}"));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).DeleteAsync("token", "en", Guid.NewGuid(), "AAAAAAAAB9E="));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
        }

        [Fact]
        public async Task GetAsync_PreservesAnHttpFailureInsteadOfReportingNotFound()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (500, "{\"message\":\"boom\"}"));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).GetAsync("token", "en", Guid.NewGuid()));

            Assert.Equal((int)HttpStatusCode.InternalServerError, exception.StatusCode);
        }
    }
}
