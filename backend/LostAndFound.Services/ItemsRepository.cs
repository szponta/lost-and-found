using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public interface IItemsRepository : IRepository<Item>;

public class ItemsRepository(LostAndFoundDbContext context) : Repository<Item>(context), IItemsRepository;

public interface IRepository<T> where T : class
{
    Task<PagedResults<T>> GetPagedEntities(int take, int skip);
}

public class Repository<T>(LostAndFoundDbContext context) where T : class
{
    public async Task<PagedResults<T>> GetPagedEntities(int take, int skip)
    {
        var dbSet = context.Set<T>();
        var totalEntities = dbSet.Count();
        var entities = await dbSet
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PagedResults<T>
        {
            Total = totalEntities,
            Count = entities.Count,
            Skip = skip,
            Take = take,
            Items = entities
        };
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

public class PaginationParameters
{
    public int Skip { get; set; }
    public int Take { get; set; }
}