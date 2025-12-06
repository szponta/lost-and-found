using FluentAssertions;
using LostAndFound.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LostAndFound.Tests.Unit;

public class GetItemsTests
{
    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly IGetItemsHandler _handler;

    public GetItemsTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_context);
        services.AddLostAndFoundServices();
        var serviceProvider = services.BuildServiceProvider();
        _handler = serviceProvider.GetRequiredService<IGetItemsHandler>();
    }

    [Fact]
    public async Task GetItems_Test()
    {
        // Arrange
        var itemId = 2333;

        new DbBuilder(_context)
            .AddItem(itemId)
            .WithName("Test Item")
            .WithDescription("Test Description")
            .AddDetail()
            .WithKey("Color")
            .WithValue("Red")
            .Build();

        // Act
        var response = await _handler.HandleAsync();

        // Assert
        response.Should().NotBeNull();
        response.Items.Should().NotBeNullOrEmpty();
        var items = response.Items.ToList();
        items.Count.Should().Be(1);
        items[0].Id.Should().Be(itemId);
        items[0].Name.Should().Be("Test Item");
        items[0].Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task GetItems_GivenSearchValue_ShouldReturnFilteredItems()
    {
        // Arrange
        new DbBuilder(_context)
            .AddItem()
            .WithName("Portfel")
            .WithDescription("Skórzany portfel")
            .AddItem()
            .WithName("Czapka")
            .WithDescription("Ciepła czapka zimowa")
            .Build();

        // Act
        var response = await _handler.HandleAsync(search: "portfel");

        // Assert
        response.Should().NotBeNull();
        response.Items.Should().NotBeNullOrEmpty();
        var items = response.Items.ToList();
        items.Count.Should().Be(1);
        items[0].Name.Should().Be("Portfel");
    }

    [Fact]
    public async Task GetItems_GivenSearch_SearchAlsoByDescription()
    {
        // Arrange
        new DbBuilder(_context)
            .AddItem()
            .WithName("Portfel")
            .WithDescription("Skórzany portfel")
            .AddDetail().WithKey("Kolor").WithValue("Czarny")
            .AddItem()
            .WithName("Portfel")
            .WithDescription("Plastikowy portfel")
            .AddDetail().WithKey("Kolor").WithValue("Czerwony")
            .Build();

        // Act
        var response = await _handler.HandleAsync(search: "czarny portfel");

        // Assert
        response.Should().NotBeNull();
        response.Items.Should().NotBeNullOrEmpty();
        var items = response.Items.ToList();
        items.Count.Should().NotBe(0);
        items[0].Name.Should().Be("Portfel");
        items[0].Description.Should().Be("Skórzany portfel");
    }
}