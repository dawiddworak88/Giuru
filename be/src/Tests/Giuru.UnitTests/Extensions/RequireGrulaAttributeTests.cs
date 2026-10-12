using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Filters;
using System;
using System.Collections.Generic;

namespace Giuru.UnitTests.Extensions
{
    public class RequireGrulaAttributeTests
    {
        private static ResourceExecutingContext CreateContext(AppSettings settings)
        {
            var services = new ServiceCollection().AddSingleton(Options.Create(settings)).BuildServiceProvider();
            var httpContext = new DefaultHttpContext { RequestServices = services };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            return new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), new List<IValueProviderFactory>());
        }

        [Fact]
        public void OnResourceExecuting_WhenGrulaIsNotConfigured_AnswersNotFound()
        {
            var context = CreateContext(new AppSettings());

            new RequireGrulaAttribute().OnResourceExecuting(context);

            Assert.IsType<NotFoundResult>(context.Result);
        }

        [Theory]
        [InlineData("token", null)]
        [InlineData("token", "not-a-guid")]
        [InlineData(null, "5f0c7d3e-3a5a-4a6b-8a54-1b0a7c7d2f10")]
        [InlineData("  ", "5f0c7d3e-3a5a-4a6b-8a54-1b0a7c7d2f10")]
        public void OnResourceExecuting_WhenTheGrulaSettingsAreIncomplete_AnswersNotFound(string accessToken, string environmentId)
        {
            var context = CreateContext(new AppSettings { GrulaAccessToken = accessToken, GrulaEnvironmentId = environmentId });

            new RequireGrulaAttribute().OnResourceExecuting(context);

            Assert.IsType<NotFoundResult>(context.Result);
        }

        [Fact]
        public void OnResourceExecuting_WhenGrulaIsConfigured_PassesThrough()
        {
            var context = CreateContext(new AppSettings { GrulaAccessToken = "token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            new RequireGrulaAttribute().OnResourceExecuting(context);

            Assert.Null(context.Result);
        }
    }
}
