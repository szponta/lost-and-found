using System.Net.Http.Json;
using FluentAssertions;
using LostAndFound.API;
using LostAndFound.Contracts;
using LostAndFound.Services;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class GetSingleItemFromDatasourceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly Mock<IDataSeeder> _dataSeeder = new();

    public GetSingleItemFromDatasourceTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _dataSeeder.Setup(x => x.Seed()).Returns(Task.CompletedTask);

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
    public async Task GetSingleItemTest()
    {
        // Arrange
        var itemId = 1;

        // Act
        var item = await _client.GetFromJsonAsync<ItemDetailsResponse>($"/api/v1/items/{itemId}");

        // Assert
        item.Should().NotBeNull();
        item.Id.Should().Be(itemId);
    }
}