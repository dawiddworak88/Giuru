﻿using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Images
{
    public class MockAuthImage : IImage
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IImage _image = new DockerImage("testcontainers", "mock-auth", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

        public async Task InitializeAsync()
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                await new ImageFromDockerfileBuilder()
                    .WithName(this)
                    .WithDockerfileDirectory(Path.Combine(CommonDirectoryPath.GetProjectDirectory().DirectoryPath, "../../"))
                    .WithDockerfile("Tests/Giuru.MockAuth/Dockerfile")
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