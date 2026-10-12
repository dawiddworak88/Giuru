using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.DiscountCodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BuyerAppSettings = Buyer.Web.Shared.Configurations.AppSettings;
using BuyerClientApiDiscountCodeLookup = Buyer.Web.Shared.Services.DiscountCodes.ClientApiDiscountCodeLookup;
using BuyerDiscountCodesRepository = Buyer.Web.Shared.Repositories.Clients.DiscountCodesRepository;
using BuyerIDiscountCodesRepository = Buyer.Web.Shared.Repositories.Clients.IDiscountCodesRepository;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerClientApiDiscountCodeLookup = Seller.Web.Shared.Services.DiscountCodes.ClientApiDiscountCodeLookup;
using SellerDiscountCodesRepository = Seller.Web.Areas.Clients.Repositories.DiscountCodes.DiscountCodesRepository;
using SellerIDiscountCodesRepository = Seller.Web.Areas.Clients.Repositories.DiscountCodes.IDiscountCodesRepository;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    /// <summary>A stand-in for Client.Api on a local port, recording what the web apps actually sent.</summary>
    internal sealed class FakeClientApi : IAsyncDisposable
    {
        private readonly WebApplication _app;

        private FakeClientApi(WebApplication app, string url)
        {
            _app = app;
            Url = url;
        }

        public string Url { get; }

        public List<RecordedRequest> Requests { get; } = new();

        public sealed record RecordedRequest(string Method, string Path, string RawQuery, IReadOnlyDictionary<string, StringValues> Query, string Body);

        public static async Task<FakeClientApi> StartAsync(Func<RecordedRequest, (int StatusCode, string Body)> respond)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();

            var app = builder.Build();
            app.Urls.Add("http://127.0.0.1:0");
            FakeClientApi fake = null;

            app.Run(async context =>
            {
                string body;

                using (var reader = new StreamReader(context.Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                var recorded = new RecordedRequest(
                    context.Request.Method,
                    context.Request.Path.Value,
                    context.Request.QueryString.Value,
                    QueryHelpers.ParseQuery(context.Request.QueryString.Value),
                    body);

                fake.Requests.Add(recorded);

                var (statusCode, responseBody) = respond(recorded);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                if (responseBody is not null)
                {
                    await context.Response.WriteAsync(responseBody);
                }
            });

            await app.StartAsync();

            fake = new FakeClientApi(app, app.Urls.First());

            return fake;
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

    public class BuyerDiscountCodeLookupTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static BuyerDiscountCodesRepository CreateRepository(string clientUrl)
        {
            return new BuyerDiscountCodesRepository(new ApiClientService(), Options.Create(new BuyerAppSettings { ClientUrl = clientUrl }));
        }

        [Fact]
        public async Task ValidateAsync_AsksTheValidationEndpointForTheClientAndMapsAValidAnswer()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"Valid\",\"discountCode\":\"SUMMER25\"}"));

            var validation = await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "summer25");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);

            var request = Assert.Single(api.Requests);
            Assert.Equal("GET", request.Method);
            Assert.Equal("/api/v1/discountcodes/validation", request.Path);
            Assert.Equal("summer25", request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
        }

        [Fact]
        public async Task ValidateAsync_SendsACodeWithSpecialCharactersEncodedSoItCannotChangeTheQuery()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"NotFound\"}"));
            const string code = "A&ClientId=00000000-0000-0000-0000-000000000000 +/ü#?%20";

            await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, code);

            var request = Assert.Single(api.Requests);
            Assert.Equal(code, request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
            Assert.DoesNotContain(request.Query.Keys, key => key == "A");
        }

        [Theory]
        [InlineData("NotApplicable", DiscountCodeValidationStatus.NotApplicable)]
        [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
        [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
        [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
        [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData("Surprise", DiscountCodeValidationStatus.Unavailable)]
        [InlineData("0", DiscountCodeValidationStatus.Unavailable)]
        public async Task ValidateAsync_MapsTheStatusStrictly(string status, DiscountCodeValidationStatus expected)
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, $"{{\"status\":\"{status}\",\"discountCode\":\"SUMMER25\"}}"));

            var validation = await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "SUMMER25");

            Assert.Equal(expected, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheBodyIsMissing_IsUnavailableNotAnAnswer()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, null));

            var validation = await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "SUMMER25");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task ValidateAsync_WhenTheApiFails_ThrowsSoThatItIsNeverReadAsAnAnswer(HttpStatusCode statusCode)
        {
            await using var api = await FakeClientApi.StartAsync(_ => ((int)statusCode, "{\"message\":\"nope\"}"));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "SUMMER25"));

            Assert.Equal((int)statusCode, exception.StatusCode);
        }

        [Fact]
        public async Task TheLookup_PassesTheClientCodeTokenAndCurrentCultureToTheRepository()
        {
            var repository = Substitute.For<BuyerIDiscountCodesRepository>();
            repository.ValidateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));
            var previous = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("pl");

                var validation = await new BuyerClientApiDiscountCodeLookup(repository).ValidateAsync(ClientId, "summer25", "token");

                Assert.True(validation.IsValid);
                await repository.Received(1).ValidateAsync("token", "pl", ClientId, "summer25");
            }
            finally
            {
                CultureInfo.CurrentUICulture = previous;
            }
        }

        [Fact]
        public async Task TheLookupAndTheValidator_TurnAFailedCallIntoAnUnverifiedCode()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (500, "{\"message\":\"boom\"}"));
            var lookup = new BuyerClientApiDiscountCodeLookup(CreateRepository(api.Url));
            var validator = new DiscountCodeValidator(lookup, Substitute.For<ILogger<DiscountCodeValidator>>());

            var validation = await validator.ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }
    }

    public class SellerDiscountCodeLookupTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static SellerDiscountCodesRepository CreateRepository(string clientUrl)
        {
            return new SellerDiscountCodesRepository(new ApiClientService(), Options.Create(new SellerAppSettings { ClientUrl = clientUrl }));
        }

        [Fact]
        public async Task ValidateAsync_AsksTheValidationEndpointForTheClientAndMapsAValidAnswer()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"Valid\",\"discountCode\":\"SUMMER25\"}"));

            var validation = await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "summer25");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);

            var request = Assert.Single(api.Requests);
            Assert.Equal("/api/v1/discountcodes/validation", request.Path);
            Assert.Equal("summer25", request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
        }

        [Fact]
        public async Task ValidateAsync_SendsACodeWithSpecialCharactersEncodedSoItCannotChangeTheQuery()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"NotFound\"}"));
            const string code = "A&ClientId=00000000-0000-0000-0000-000000000000 +/ü#?%20";

            await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, code);

            var request = Assert.Single(api.Requests);
            Assert.Equal(code, request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
        }

        [Theory]
        [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
        [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
        [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
        [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData("Surprise", DiscountCodeValidationStatus.Unavailable)]
        public async Task ValidateAsync_MapsTheStatusStrictly(string status, DiscountCodeValidationStatus expected)
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, $"{{\"status\":\"{status}\"}}"));

            var validation = await CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "SUMMER25");

            Assert.Equal(expected, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheApiFails_Throws()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (500, "{\"message\":\"boom\"}"));

            await Assert.ThrowsAsync<CustomException>(() => CreateRepository(api.Url).ValidateAsync("token", "en", ClientId, "SUMMER25"));
        }

        [Fact]
        public async Task TheLookup_PassesTheClientCodeTokenAndCurrentCultureToTheRepository()
        {
            var repository = Substitute.For<SellerIDiscountCodesRepository>();
            repository.ValidateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled)));
            var previous = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("de");

                var validation = await new SellerClientApiDiscountCodeLookup(repository).ValidateAsync(ClientId, "summer25", "token");

                Assert.Equal(DiscountCodeValidationStatus.Disabled, validation.Status);
                await repository.Received(1).ValidateAsync("token", "de", ClientId, "summer25");
            }
            finally
            {
                CultureInfo.CurrentUICulture = previous;
            }
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
