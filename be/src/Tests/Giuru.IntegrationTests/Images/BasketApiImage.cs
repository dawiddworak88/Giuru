using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Images
{
    public class BasketApiImage : IImage
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IImage _image = new DockerImage("testcontainers", "basket-api", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        public async Task InitializeAsync()
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                await new ImageFromDockerfileBuilder()
                    .WithName(this)
                    .WithDockerfileDirectory(Path.Combine(CommonDirectoryPath.GetProjectDirectory().DirectoryPath, "../../"))
                    .WithDockerfile("Project/Services/Basket/Basket.Api/Dockerfile")
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
