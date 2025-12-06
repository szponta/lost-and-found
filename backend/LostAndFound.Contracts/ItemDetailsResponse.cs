namespace LostAndFound.Contracts;

public class ItemDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";

    public DateTime? LostDateFrom { get; set; }
    public DateTime? LostDateTo { get; set; }

    public DateTime? FoundDate { get; set; }

    public string? EventLocation { get; set; }
    public string? StorageLocation { get; set; }

    public string? City { get; set; }

    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public IList<ItemDetailsResponseDetail> Details { get; set; } = [];
}

public class ItemDetailsResponseDetail
{
    public string Key { get; set; }
    public string Value { get; set; }
}