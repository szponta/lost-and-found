using System.Text.RegularExpressions;
using AutoMapper;
using LostAndFound.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public interface IGetItemsHandler
{
    public Task<ItemsResponse> HandleAsync(int take = 10, int skip = 0,
        string search = "",
        DateTime? foundDateFrom = null,
        DateTime? foundDateTo = null,
        string country = "",
        string location = ""
    );
}

public partial class GetItemsHandler(LostAndFoundDbContext context) : IGetItemsHandler
{
    private readonly IMapper _mapper =
        new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Item, ItemsResponseItem>()
                .ForMember(dest => dest.LostDateFrom,
                    opt => opt.MapFrom(src => src.LostDateFrom.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.LostDateTo,
                    opt => opt.MapFrom(src => src.LostDateTo.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.FoundDate,
                    opt => opt.MapFrom(src => src.FoundDate.GetValueOrDefault().DateTime))
                ;

            cfg.CreateMap<PagedResults<Item>, ItemsResponse>()
                ;
        }).CreateMapper();

    public async Task<ItemsResponse> HandleAsync(int take = 10, int skip = 0, string search = "",
        DateTime? foundDateFrom = null,
        DateTime? foundDateTo = null,
        string country = "",
        string location = ""
    )
    {
        var dbSet = context.Set<Item>()
            .Select(item => new SearchItem
            {
                Item = item, Search = $"{item.Name} {item.Description}".ToLowerInvariant(),
                Country = item.Country.ToLowerInvariant(),
                Location = item.EventLocation != null ? item.EventLocation.ToLowerInvariant() : "",
                DetailSearch = context.Set<ItemDetail>()
                    .Where(d => d.ItemId == item.Id)
                    .Select(d => d.Value.ToLowerInvariant())
                    .ToList()
            });

        if (!string.IsNullOrWhiteSpace(country))
        {
            country = country.ToLowerInvariant();
            dbSet = dbSet.Where(x => x.Country == country);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLowerInvariant();
            search = MyRegex().Replace(search, " ").Trim();

            var words = search
                .Split(' ')
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => w.ToLowerInvariant())
                .ToList();

            var word1 = words.Count > 0 ? words[0] : "";
            var word2 = words.Count > 1 ? words[1] : "";
            var word3 = words.Count > 2 ? words[2] : "";
            var word4 = words.Count > 3 ? words[3] : "";
            var word5 = words.Count > 4 ? words[4] : "";

            dbSet = dbSet.Where(x =>
                (word1 != "" && x.Search.Contains(word1)) ||
                (word2 != "" && x.Search.Contains(word2)) ||
                (word3 != "" && x.Search.Contains(word3)) ||
                (word4 != "" && x.Search.Contains(word4)) ||
                (word5 != "" && x.Search.Contains(word5)));

            // count how many times each word appears in details
            dbSet = dbSet.Select(x => new SearchItem
            {
                Item = x.Item,
                Search = x.Search,
                Value = x.DetailSearch.Count(y =>
                    (word1 != "" && y.Contains(word1)) ||
                    (word2 != "" && y.Contains(word2)) ||
                    (word3 != "" && y.Contains(word3)) ||
                    (word4 != "" && y.Contains(word4)) ||
                    (word5 != "" && y.Contains(word5))
                )
            }).OrderByDescending(x => x.Value);
        }

        if (foundDateFrom != null)
        {
            dbSet = dbSet.Where(x => x.Item.FoundDate != null && x.Item.FoundDate >= foundDateFrom);
        }

        if (foundDateTo != null)
        {
            dbSet = dbSet.Where(x => x.Item.FoundDate != null && x.Item.FoundDate <= foundDateTo);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            location = location.ToLowerInvariant();
            dbSet = dbSet.Where(x => x.Location.Contains(location));
        }

        var totalEntities = dbSet.Count();
        var items = await dbSet.Select(x => x.Item)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var itemsResult = new PagedResults<Item>
        {
            Total = totalEntities,
            Count = items.Count,
            Skip = skip,
            Take = take,
            Items = items
        };
        return _mapper.Map<ItemsResponse>(itemsResult);
    }


    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();

    private class SearchItem
    {
        public Item Item { get; set; } = null!;
        public string Search { get; set; } = null!;
        public IList<string> DetailSearch { get; set; } = [];
        public int Value { get; set; }
        public string Country { get; set; } = "";
        public string Location { get; set; } = "";
    }
}

public class PagedResults<T>
{
    public int Total { get; set; }
    public int Count { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public IList<T> Items { get; set; } = [];
}