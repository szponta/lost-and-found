using AutoMapper;
using LostAndFound.Contracts;

namespace LostAndFound.Services;

public interface IGetItemsHandler
{
    public Task<ItemsResponse> HandleAsync(int take = 10, int skip = 0, string search = "");
}

public class GetItemsHandler(IItemsRepository repository) : IGetItemsHandler
{
    private readonly IMapper _mapper =
        new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Item, ItemsResponseItem>();
            cfg.CreateMap<PagedResults<Item>, ItemsResponse>();
        }).CreateMapper();

    public async Task<ItemsResponse> HandleAsync(int take = 10, int skip = 0, string search = "")
    {
        var itemsResult = await repository.GetPagedEntities(take, skip, search);
        return _mapper.Map<ItemsResponse>(itemsResult);
    }
}