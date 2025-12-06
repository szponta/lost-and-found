namespace LostAndFound.Services;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string Country { get; set; } = "";
    public string Voivodeship { get; set; } = "";
    public string City { get; set; } = "";
    public string Street { get; set; } = "";
    public string Place { get; set; } = "";

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public IList<ItemDetails> Details { get; set; } = [];
}

public class ItemDetails
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}