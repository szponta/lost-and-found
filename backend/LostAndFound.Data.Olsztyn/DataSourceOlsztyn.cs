using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LostAndFound.Data.Olsztyn;

public class DataSourceOlsztyn
{
    // Look for embedded resource names that match one of these suffixes
    private static readonly string[] ResourceSuffixes = ["data.olsztyn", "olsztyn-data.json"];

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async IAsyncEnumerable<OlsztynData> GetItems()
    {
        var asm = typeof(DataSourceOlsztyn).Assembly;
        var resources = asm.GetManifestResourceNames();

        // Prefer exact suffix matches, fall back to any name containing "plock"
        var resourceName = resources
                               .FirstOrDefault(n =>
                                   ResourceSuffixes.Any(s => n.EndsWith(s, StringComparison.OrdinalIgnoreCase)))
                           ?? resources.FirstOrDefault(n =>
                               n.IndexOf("olsztyn", StringComparison.OrdinalIgnoreCase) >= 0);

        if (resourceName == null)
        {
            yield break;
        }

        await using var stream = asm.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            yield break;
        }

        var data = await JsonSerializer.DeserializeAsync<IList<OlsztynDataInternal>>(stream, _options)
                   ?? throw new InvalidOperationException("Deserialized data is null.");

        foreach (var item in data)
            yield return new OlsztynData
            {
                Name = item.FieldNazwaDepozytu,
                Url = GetUri(item),
                ReceivedAt = GetDateTime(item)
            };
    }

    private static Uri GetUri(OlsztynDataInternal item)
    {
        var title = item.Title;
        var href = Regex.Match(title, @"href=""([^""]+)""").Groups[1].Value;
        var uri = new Uri($"https://otwartedane.olsztyn.eu{href}", UriKind.Absolute);
        return uri;
    }

    private static DateTimeOffset? GetDateTime(OlsztynDataInternal item)
    {
        var dataPrzyjeciaDepozytu = item.FieldDataPrzyjeciaDepozytu;
        var dateTimeStr = Regex.Match(dataPrzyjeciaDepozytu, @"datetime=""([^""]+)""").Groups[1].Value;
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            return null;
        }

        var dateTime = DateTimeOffset.Parse(dateTimeStr);
        return dateTime;
    }
}

public class OlsztynData
{
    public string Name { get; set; } = "";
    public DateTimeOffset? ReceivedAt { get; set; }
    public Uri? Url { get; set; }
}