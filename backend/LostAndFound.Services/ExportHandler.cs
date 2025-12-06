using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public interface IExportHandler
{
    Task<IResult> GetStream(ExportType type);
}

public class ExportHandler(LostAndFoundDbContext context) : IExportHandler
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public async Task<IResult> GetStream(ExportType type)
    {
        var items = await context.Items
            .Include(x => x.Details)
            .ToListAsync();

        var stream = type switch
        {
            ExportType.Json => await GetJsonStream(items),
            ExportType.Csv => await GetCsvStream(items),
            ExportType.Xml => await GetXmlStream(items),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        var contentType = type switch
        {
            ExportType.Json => "application/json",
            ExportType.Csv => "text/csv",
            ExportType.Xml => "application/xml",
            _ => "application/octet-stream"
        };

        return Results.File(stream, contentType, $"export.{type.ToString().ToLower()}", enableRangeProcessing: true);
    }

    private static async Task<Stream> GetXmlStream(List<Item> items)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        var xmlSerializer = new XmlSerializer(typeof(List<Item>));
        xmlSerializer.Serialize(writer, items);
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }

    private static async Task<Stream> GetCsvStream(List<Item> items)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync("Id,Name,Description");
        foreach (var item in items) await writer.WriteLineAsync($"{item.Id},{item.Name},{item.Description}");
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }

    private static async Task<Stream> GetJsonStream(List<Item> items)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        var json = JsonSerializer.Serialize(items, Options);
        await writer.WriteAsync(json);
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }
}

public enum ExportType
{
    Json,
    Csv,
    Xml
}