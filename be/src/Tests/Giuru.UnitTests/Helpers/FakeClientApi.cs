using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Helpers
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

        public sealed record RecordedRequest(
            string Method,
            string Path,
            string RawQuery,
            IReadOnlyDictionary<string, StringValues> Query,
            string Body,
            string Authorization,
            string AcceptLanguage);

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
                    body,
                    context.Request.Headers.Authorization.ToString(),
                    context.Request.Headers.AcceptLanguage.ToString());

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
}
