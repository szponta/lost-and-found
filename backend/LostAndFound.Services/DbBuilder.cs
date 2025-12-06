namespace LostAndFound.Services;

public class DbBuilder(LostAndFoundDbContext context)
{
    private readonly LostAndFoundDbContext _context = context;

    public DbItemBuilder AddItem(int id = 0)
    {
        var item = new Item
        {
            Id = id
        };

        _context.Add(item);

        return new DbItemBuilder(this);
    }

    public void Build()
    {
        _context.SaveChanges();
    }

    public class DbItemBuilder(DbBuilder builder) : DbBuilder(builder._context)
    {
        public DbItemBuilder WithTitle(string title)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Title = title;
            return this;
        }

        public DbItemBuilder WithDescription(string description)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Description = description;
            return this;
        }
    }
}