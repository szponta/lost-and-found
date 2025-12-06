using System.Net.Http.Json;
using FluentAssertions;
using LostAndFound.Contracts;
using LostAndFound.Data.Plock;
using LostAndFound.Services;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class GetItemsIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    public GetItemsIntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper, false));
                });

                builder.ConfigureServices(services => { services.AddSingleton(_context); });
            }).CreateClient();
    }

    [Fact]
    public async Task GetItemsTest()
    {
        // Arrange
        var itemId = 2333;

        new DbBuilder(_context)
            .AddItem(itemId)
            .WithTitle("Test Item")
            .WithDescription("Test Description")
            .Build();

        // Act
        var response = await _client.GetFromJsonAsync<ItemsResponse>("/api/v1/items");

        // Assert
        response.Should().NotBeNull();
        response.Items.Should().NotBeNullOrEmpty();
    }

    private class Response
    {
        public string Message { get; init; } = null!;
    }
}