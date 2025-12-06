using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public interface IItemsRepository : IRepository<Item>;

public partial class ItemsRepository(LostAndFoundDbContext context) : Repository<Item>(context), IItemsRepository
{
    private readonly LostAndFoundDbContext _context = context;

    public async Task<PagedResults<Item>> GetPagedEntities(int take, int skip, string search = "")
    {
        var dbSet = _context.Set<Item>()
            .Select(item => new SearchItem
            {
                Item = item, Search = $"{item.Name} {item.Description}".ToLowerInvariant(),

                DetailSearch = _context.Set<ItemDetail>()
                    .Where(d => d.ItemId == item.Id)
                    .Select(d => d.Value.ToLowerInvariant())
                    .ToList()
            });

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

        var totalEntities = dbSet.Count();
        var items = await dbSet.Select(x => x.Item)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PagedResults<Item>
        {
            Total = totalEntities,
            Count = items.Count,
            Skip = skip,
            Take = take,
            Items = items
        };
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();

    private class SearchItem
    {
        public Item Item { get; set; } = null!;
        public string Search { get; set; } = null!;
        public IList<string> DetailSearch { get; set; } = [];
        public int Value { get; set; }
    }
}

public interface IRepository<T> where T : class
{
    Task<PagedResults<T>> GetPagedEntities(int take, int skip, string search = "");
}

public class Repository<T>(LostAndFoundDbContext context) where T : class
{
}

public class PagedResults<T>
{
    public int Total { get; set; }
    public int Count { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public IList<T> Items { get; set; } = [];
}

public class PaginationParameters
{
    public int Skip { get; set; }
    public int Take { get; set; }
}