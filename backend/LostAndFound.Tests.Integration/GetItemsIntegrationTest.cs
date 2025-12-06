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

public class GetItemsIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly Mock<IDataSeeder> _dataSeeder = new();

    public GetItemsIntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
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

                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_context);
                    services.AddSingleton(_dataSeeder.Object);
                });
            }).CreateClient();
    }

    [Fact]
    public async Task GetItemsTest()
    {
        // Arrange
        var itemId = 2333;

        new DbBuilder(_context)
            .AddItem(itemId)
            .WithName("Test Item")
            .WithDescription("Test Description")
            .WithCity("Plock")
            .WithStatus(ItemStatus.Lost)
            .WithLostDateFrom(new DateTime(2020, 01, 01))
            .WithLostDateTo(new DateTime(2020, 01, 02))
            .Build();

        // Act
        var response = await _client.GetFromJsonAsync<ItemsResponse>("/api/v1/items");

        // Assert
        response.Should().NotBeNull();
        response.Items.Should().NotBeNullOrEmpty();
        var item = response.Items.First(i => i.Id == itemId);
        item.Name.Should().Be("Test Item");
        item.Description.Should().Be("Test Description");
        item.City.Should().Be("Plock");
        item.Status.Should().Be(nameof(ItemStatus.Lost));
        item.LostDateFrom.Should().Be(new DateTime(2020, 01, 01));
        item.LostDateTo.Should().Be(new DateTime(2020, 01, 02));
    }
}