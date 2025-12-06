using FluentAssertions;
using LostAndFound.Data.Olsztyn;

namespace LostAndFound.Tests.Integration;

public class DataSourceOlsztynTests
{
    private readonly DataSourceOlsztyn _sut = new();

    [Fact]
    public async Task GetDataTest()
    {
        // Arrange

        // Act
        var data = await _sut.GetItems().ToListAsync();

        // Assert
        data.Should().NotBeNull();
        data.Should().NotBeEmpty();
        // var item = data.FirstOrDefault(d => d.Id == 58648);
        // item.Should().NotBeNull();
        // item.Title.Should().Be("BRZ - wózek inwalidzki");
        // item.Url.Should().Be(new Uri("https://przemysl.skycms.com.pl/58648/brz-wozek-inwalidzki.html"));
        // item.Preamble.Should().BeNullOrEmpty();
        // item.Content.Should().BeNullOrEmpty();
        // item.CreatedAt.Should().Be(new DateTime(2020, 8, 24, 7, 57, 18));
        // item.UpdatedAt.Should().Be(new DateTime(2021, 11, 23, 7, 37, 50));
        // item.ContentTypeName.Should().Be("Biuro Rzeczy Znalezionych");
        // item.ContentTypeFields.Should().HaveCount(3);
        // item.ContentTypeFields[0].NodeName.Should().Be("Nazwa rzeczy znalezionej");
        // item.ContentTypeFields[0].NodeValue.Should().Be("Wózek inwalidzki");
        // item.ContentTypeFields[1].NodeName.Should().Be("Data przekazania do biura");
        // item.ContentTypeFields[1].NodeValue.Should().Be("2020-08-20");
        // item.ContentTypeFields[2].NodeName.Should().Be("Przekazujący");
        // item.ContentTypeFields[2].NodeValue.Should().Be("Straż Miejska w Przemyślu");
    }
}