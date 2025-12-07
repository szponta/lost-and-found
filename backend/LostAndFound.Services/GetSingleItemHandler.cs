using AutoMapper;
using LostAndFound.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public interface IGetSingleItemHandler
{
    public Task<ItemDetailsResponse> HandleAsync(int itemId);
}

public class GetSingleItemHandler(LostAndFoundDbContext context) : IGetSingleItemHandler
{
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Item, ItemDetailsResponse>()
            .ForMember(dest => dest.LostDateFrom,
                opt => opt.MapFrom(src => src.LostDateFrom.GetValueOrDefault().DateTime))
            .ForMember(dest => dest.LostDateTo, opt => opt.MapFrom(src => src.LostDateTo.GetValueOrDefault().DateTime))
            .ForMember(dest => dest.FoundDate, opt => opt.MapFrom(src => src.FoundDate.GetValueOrDefault().DateTime))
            ;
        cfg.CreateMap<ItemDetail, ItemDetailsResponseDetail>();
    }).CreateMapper();

    public async Task<ItemDetailsResponse> HandleAsync(int itemId)
    {
        var item = await context.Items
            .Include(x => x.Details)
            .Where(i => i.Id == itemId)
            .FirstOrDefaultAsync();

        if (item == null)
        {
            throw new KeyNotFoundException();
        }

        var response = _mapper.Map<ItemDetailsResponse>(item);
        response.Details = item.Details
            .Select(d => _mapper.Map<ItemDetailsResponseDetail>(d))
            .ToList();

        return response;
    }
}