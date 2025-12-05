using FluentAssertions;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public UnitTest1(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(x =>
               {
                   x.ClearProviders();
                   x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper, false));
               });

                builder.ConfigureServices(services =>
                {
                });
            }).CreateClient();
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act
        var response = await client.GetStringAsync("/api/v1/test");

        // Assert
        response.Should().BeEquivalentTo("hello world");
    }
}
