namespace LostAndFound.Services;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ItemStatus Status { get; set; }

    public DateTime? LostDateFrom { get; set; }
    public DateTime? LostDateTo { get; set; }

    public DateTime? FoundDate { get; set; }

    public string? EventLocation { get; set; }
    public string? StorageLocation { get; set; }

    public string? City { get; set; }
    public string Country { get; set; } = "Polska";

    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public IList<ItemEvent> Events { get; set; } = [];
    public IList<ItemDetail> Details { get; set; } = [];
}

public class ItemDetail
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public Item Item { get; set; } = null!;
}

public class ItemEvent
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public ItemEventType EventType { get; set; }
    public string? Notes { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public Item Item { get; set; } = null!;
}

public enum ItemEventType
{
    Unknown = 0,
    ReportedLost = 1,
    ReportedFound = 2,
    ReturnedToOwner = 3,
    Other = 4
}

public enum ItemStatus
{
    Unknown = 0,
    Lost = 1,
    Found = 2,
    Returned = 3
}