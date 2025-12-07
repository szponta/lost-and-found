using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using LostAndFound.API;
using LostAndFound.Contracts;
using LostAndFound.Services;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class PostItemTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly Mock<IDataSeeder> _dataSeeder = new();
    private readonly FakeTimeProvider _fakeTime = new();

    public PostItemTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
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
                    services.AddSingleton<TimeProvider>(_fakeTime);
                });
            }).CreateClient();
    }

    [Fact]
    public async Task PostItem_Test()
    {
        // Arrange
        var request = new CreateItemRequest
        {
            Name = "Portfel",
            Status = nameof(ItemStatus.Lost),
            Description = "Skórzany portfel",
            City = "Warszawa",
            EventLocation = "Hale Banacha, Ochota",
            LostDateFrom = new DateTime(2024, 01, 01),
            LostDateTo = new DateTime(2024, 01, 02)
        };

        _fakeTime.AdjustTime(new DateTime(2025, 12, 06, 12, 11, 22));

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/items", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<CreateItemResponse>();
        responseData.Should().NotBeNull();
        var id = responseData!.Id;
        id.Should().BeGreaterThan(0);
        var item = await _context.Items.FindAsync(id);
        item.Should().NotBeNull();
        item.Name.Should().Be("Portfel");
        item.Status.Should().Be(ItemStatus.Lost);
        item.Description.Should().Be("Skórzany portfel");
        item.City.Should().Be("Warszawa");
        item.EventLocation.Should().Be("Hale Banacha, Ochota");
        item.LostDateFrom.Should().Be(new DateTime(2024, 01, 01));
        item.LostDateTo.Should().Be(new DateTime(2024, 01, 02));

        item.CreatedAt.Should().Be(new DateTime(2025, 12, 06, 12, 11, 22));
    }

    [Fact]
    public async Task PostItemDetails_GivenExistingItem_ShouldAddDetails()
    {
        // Arrange
        var itemId = 3456;
        new DbBuilder(_context)
            .AddItem(itemId)
            .WithName("Test Item")
            .WithDescription("Test Description")
            .WithCreatedAt(new DateTimeOffset(2025, 01, 01, 10, 10, 10, 10, TimeSpan.Zero))
            .AddDetail().WithKey("Size").WithValue("Large")
            .WithDetailCreatedAt(new DateTimeOffset(2025, 02, 02, 10, 10, 10, 10, TimeSpan.Zero))
            .Build();

        var requestItem1 = new CreateItemDetailsRequest { Key = "Color", Value = "Red" };
        var requestItem2 = new CreateItemDetailsRequest { Key = "Material", Value = "Leather" };

        _fakeTime.AdjustTime(new DateTimeOffset(2025, 12, 06, 11, 11, 11, 11, TimeSpan.Zero));

        // Act
        var response = await _client.PostAsJsonAsync<IList<CreateItemDetailsRequest>>($"/api/v1/items/{itemId}/details",
            [requestItem1, requestItem2]);

        // Assert
        response.EnsureSuccessStatusCode();
        var item = await _context.Items.FindAsync(itemId);
        item.Should().NotBeNull();

        item.CreatedAt.Should().Be(new DateTimeOffset(2025, 01, 01, 10, 10, 10, 10, TimeSpan.Zero));
        item.UpdatedAt.Should().Be(new DateTimeOffset(2025, 12, 06, 11, 11, 11, 11, TimeSpan.Zero));

        var details = _context.ItemDetails.Where(d => d.ItemId == itemId).ToList();
        details.Count.Should().Be(3);
        details.Should().ContainSingle(d => d.Key == "Color" && d.Value == "Red");
        details.Should().ContainSingle(d => d.Key == "Material" && d.Value == "Leather");
        details.Should().ContainSingle(d => d.Key == "Size" && d.Value == "Large");

        var colorDetail = details.Single(d => d.Key == "Color");
        var materialDetail = details.Single(d => d.Key == "Material");
        var sizeDetail = details.Single(d => d.Key == "Size");

        colorDetail.CreatedAt.Should().Be(new DateTimeOffset(2025, 12, 06, 11, 11, 11, 11, TimeSpan.Zero));
        materialDetail.CreatedAt.Should().Be(new DateTimeOffset(2025, 12, 06, 11, 11, 11, 11, TimeSpan.Zero));
        sizeDetail.CreatedAt.Should().Be(new DateTimeOffset(2025, 02, 02, 10, 10, 10, 10, TimeSpan.Zero));
    }

    [Fact]
    public async Task PostAndGet_Test()
    {
        // Arrange
        var request =
            @"{
                ""name"":""kopytko"",
                ""status"":""lost"",
                ""eventLocation"":""Park Centralny"",
                ""storageLocation"":""Posterunek policji"",
                ""city"":""Warszawa"",
                ""country"":""Polska"",
                ""lostDateFrom"":""2023-12-06"",
                ""lostDateTo"":""2025-12-07""
            }";

        var postResponse = await _client.PostAsync("/api/v1/items",
            new StringContent(request, Encoding.UTF8, "application/json"));
        postResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await _client.GetFromJsonAsync<ItemsResponse>(
            "/api/v1/items/?take=20&skip=0&search=kopytko&foundDateFrom=2023-12-06&foundDateTo=2025-12-07&country=Polska&city=Warszawa");

        // Assert
        getResponse.Should().NotBeNull();
        getResponse.Items.Should().NotBeNullOrEmpty();
    }
}