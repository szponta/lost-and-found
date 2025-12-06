using System.Text.Json;
using FluentAssertions;
using LostAndFound.API;
using LostAndFound.Services;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace LostAndFound.Tests.Integration;

public class ExportTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly LostAndFoundDbContext _context = DbFactory.GetInMemoryDbContext();

    private readonly Mock<IDataSeeder> _dataSeeder = new();

    public ExportTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
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
    public async Task GetJsonExportTest()
    {
        // Arrange
        new DbBuilder(_context)
            .AddItem()
            .WithName("Test item")
            .WithDescription("Test description")
            .AddDetail().WithKey("test_key").WithValue("test_value")
            .AddDetail().WithKey("test_other_key").WithValue("test_other_value")
            .Build();

        // Act
        var stream = await _client.GetStreamAsync("/api/v1/export/json");

        // Assert
        var export = await JsonSerializer.DeserializeAsync<IList<Item>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        export.Should().NotBeNull();
        export.Should().HaveCount(1);
        var exportItem = export[0];
        exportItem.Name.Should().Be("Test item");
        exportItem.Description.Should().Be("Test description");
        exportItem.Details.Should().HaveCount(2);
        exportItem.Details[0].Key.Should().Be("test_key");
        exportItem.Details[0].Value.Should().Be("test_value");
        exportItem.Details[1].Key.Should().Be("test_other_key");
        exportItem.Details[1].Value.Should().Be("test_other_value");
    }
}