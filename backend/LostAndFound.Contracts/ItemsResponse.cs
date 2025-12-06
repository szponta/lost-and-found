namespace LostAndFound.Contracts;

public class ItemsResponse
{
    public IList<ItemsResponseItem> Items { get; set; } = [];
}

public class ItemsResponseItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";
    public string EventLocation { get; set; } = "";
    public string StorageLocation { get; set; } = "";
    public string City { get; set; } = "";

    public DateTime? LostDateFrom { get; set; }
    public DateTime? LostDateTo { get; set; }
}