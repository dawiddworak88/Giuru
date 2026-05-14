using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Images
{
    public class CatalogBackgroundTasksImage : IImage
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IImage _image = new DockerImage("testcontainers", "catalog-background-tasks", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        public async Task InitializeAsync()
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                await new ImageFromDockerfileBuilder()
                    .WithName(this)
                    .WithDockerfileDirectory(Path.Combine(CommonDirectoryPath.GetProjectDirectory().DirectoryPath, "../../"))
                    .WithDockerfile("Project/Services/Catalog/Catalog.BackgroundTasks/Dockerfile")
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
        public string Tag => _image.Tag;

        public string FullName => _image.FullName;
        public string Registry => _image.Registry;

        public string Digest => _image.Digest;

        public string Platform => _image.Platform;

        public string GetHostname()
        {
            return _image.GetHostname();
        }
        public bool MatchLatestOrNightly()
        {
            return _image.MatchLatestOrNightly();
        }
        public bool MatchVersion(Predicate<string> predicate)
        {
            return _image.MatchVersion(predicate);
        }
        public bool MatchVersion(Predicate<Version> predicate)
        {
            return _image.MatchVersion(predicate);
        }
    }
}
