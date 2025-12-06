using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostAndFound.Data.Plock;

public class DataSourcePlock
{
    // Look for embedded resource names that match one of these suffixes
    private static readonly string[] ResourceSuffixes = ["data.plock", "plock-data.json"];

    private readonly JsonSerializerOptions _options;

    public DataSourcePlock()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _options.Converters.Add(new NullableDateTimeConverter());
    }

    public async IAsyncEnumerable<PlockDataItem> GetItems()
    {
        var asm = typeof(DataSourcePlock).Assembly;
        var resources = asm.GetManifestResourceNames();

        // Prefer exact suffix matches, fall back to any name containing "plock"
        var resourceName = resources
                               .FirstOrDefault(n =>
                                   ResourceSuffixes.Any(s => n.EndsWith(s, StringComparison.OrdinalIgnoreCase)))
                           ?? resources.FirstOrDefault(n =>
                               n.IndexOf("plock", StringComparison.OrdinalIgnoreCase) >= 0);

        if (resourceName == null)
        {
            yield break;
        }

        await using var stream = asm.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            yield break;
        }

        var data = await JsonSerializer.DeserializeAsync<IList<PlockDataItem>>(stream, _options)
                   ?? throw new InvalidOperationException("Deserialized data is null.");

        foreach (var item in data) yield return item;
    }
}

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }

        if (str.Equals("0000-00-00 00:00:00"))
        {
            return null;
        }

        return DateTime.ParseExact(str, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}