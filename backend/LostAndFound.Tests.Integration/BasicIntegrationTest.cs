using System.Net.Http.Json;
using FluentAssertions;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class BasicIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BasicIntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper, false));
                });

                builder.ConfigureServices(services => { });
            }).CreateClient();
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act
        var response = await _client.GetFromJsonAsync<Response>("/api/v1/test");

        // Assert
        response.Should().NotBeNull();
        response.Message.Should().BeEquivalentTo("hello world");
    }

    private class Response
    {
        public string Message { get; init; } = null!;
    }
}