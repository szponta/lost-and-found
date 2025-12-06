using AutoMapper;

namespace LostAndFound.Services;

public interface IPostItemHandler
{
    Task<CreateItemResponse> CreateItemHeaderAsync(CreateItemRequest request);
    Task CreateItemDetailsAsync(int itemId, IList<CreateItemDetailsRequest> details);
}

public class PostItemHandler(LostAndFoundDbContext context, TimeProvider timeProvider) : IPostItemHandler
{
    private readonly IMapper _mapper =
        new MapperConfiguration(cfg => { cfg.CreateMap<CreateItemRequest, Item>(); }).CreateMapper();

    public async Task<CreateItemResponse> CreateItemHeaderAsync(CreateItemRequest request)
    {
        var item = _mapper.Map<Item>(request);

        item.CreatedAt = timeProvider.GetUtcNow();

        context.Items.Add(item);
        await context.SaveChangesAsync();
        return new CreateItemResponse { Id = item.Id };
    }

    public async Task CreateItemDetailsAsync(int itemId, IList<CreateItemDetailsRequest> details)
    {
        var item = await context.Items.FindAsync(itemId);
        if (item == null)
        {
            throw new KeyNotFoundException();
        }

        var dateNow = timeProvider.GetUtcNow();
        item.UpdatedAt = dateNow;

        foreach (var detailRequest in details)
        {
            var detail = new ItemDetail
            {
                ItemId = itemId,
                Key = detailRequest.Key,
                Value = detailRequest.Value,
                CreatedAt = dateNow
            };
            context.ItemDetails.Add(detail);
        }

        await context.SaveChangesAsync();
    }
}

public class CreateItemDetailsRequest
{
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}

public class CreateItemResponse
{
    public int Id { get; set; }
}

public class CreateItemRequest
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";
    public string EventLocation { get; set; } = "";
    public string StorageLocation { get; set; } = "";
    public string City { get; set; } = "";
    public DateTime? LostDateFrom { get; set; }
    public DateTime? LostDateTo { get; set; }
    public string Country { get; set; } = "";
}