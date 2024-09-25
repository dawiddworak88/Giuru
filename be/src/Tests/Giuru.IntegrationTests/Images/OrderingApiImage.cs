using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Giuru.IntegrationTests.Images
{
    internal class OrderingApiImage : IImage
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IImage _image = new DockerImage("testcontainers", "ordering-api", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        public async Task InitializeAsync()
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                await new ImageFromDockerfileBuilder()
                    .WithName(this)
                    .WithDockerfileDirectory(Path.Combine(CommonDirectoryPath.GetProjectDirectory().DirectoryPath, "../../"))
                    .WithDockerfile("Project/Services/Ordering/Ordering.Api/Dockerfile")
                    .WithDeleteIfExists(false)
                    .Build()
                    .CreateAsync()
                    .ConfigureAwait(false);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public string Repository => _image.Repository;

        public string Name => _image.Name;

        public string Tag => _image.Tag;

        public string FullName => _image.FullName;

        public string GetHostname()
        {
            return _image.GetHostname();
        }
    }
}
