namespace LostAndFound.Contracts;

public class ItemsResponse
{
    public IList<ItemsResponseItem> Items { get; set; } = [];
}

public class ItemsResponseItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}