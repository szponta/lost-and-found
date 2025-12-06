namespace LostAndFound.Data.Plock;

public class PlockDataItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public Uri? Url { get; set; }
    public string? Preamble { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? ContentTypeName { get; set; }
    public IList<PlockDataNodeItem> ContentTypeFields { get; init; } = [];
}

public class PlockDataNodeItem
{
    public string? NodeName { get; set; }
    public string? NodeValue { get; set; }
}