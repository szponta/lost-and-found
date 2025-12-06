using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostAndFound.Data.Plock;

public interface IDataSource
{
    Task<IList<PlockDataItem>> GetItems();
}

public class DataSource
{
    public async Task<IList<PlockDataItem>> GetItems()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        options.Converters.Add(new NullableDateTimeConverter());

        await using var stream = new FileStream("plock-data.json", FileMode.Open, FileAccess.Read);
        var data = await JsonSerializer.DeserializeAsync<IList<PlockDataItem>>(stream, options) ??
                   throw new InvalidOperationException();
        return data;
    }
}

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str)) return null;
        if (str.Equals("0000-00-00 00:00:00")) return null;
        return DateTime.ParseExact(str, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Value.ToString(Format, CultureInfo.InvariantCulture));
    }
}