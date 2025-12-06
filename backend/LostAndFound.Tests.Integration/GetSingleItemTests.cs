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

public class GetSingleItemTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly Mock<IDataSeeder> _dataSeeder = new();

    public GetSingleItemTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
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
    public async Task GetSingleItemTest()
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
            .AddDetail().WithKey("Color").WithValue("Red")
            .AddDetail().WithKey("Size").WithValue("Large")
            .Build();

        // Act
        var item = await _client.GetFromJsonAsync<ItemDetailsResponse>($"/api/v1/items/{itemId}");

        // Assert
        item.Should().NotBeNull();
        item.Id.Should().Be(itemId);
        item.Name.Should().Be("Test Item");
        item.Description.Should().Be("Test Description");
        item.City.Should().Be("Plock");
        item.Status.Should().Be(nameof(ItemStatus.Lost));
        item.LostDateFrom.Should().Be(new DateTime(2020, 01, 01));
        item.LostDateTo.Should().Be(new DateTime(2020, 01, 02));

        item.Details.Should().HaveCount(2);
        item.Details.Should().ContainSingle(d => d.Key == "Color" && d.Value == "Red");
        item.Details.Should().ContainSingle(d => d.Key == "Size" && d.Value == "Large");
    }
}