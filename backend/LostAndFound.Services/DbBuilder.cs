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
        public DbItemBuilder WithName(string title)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Name = title;
            return this;
        }

        public DbItemBuilder WithDescription(string description)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Description = description;
            return this;
        }

        public DbItemDetailBuilder AddDetail()
        {
            var item = _context.Set<Item>().Local.Last();
            var detail = new ItemDetail
            {
                Item = item
            };
            _context.Add(detail);
            return new DbItemDetailBuilder(this);
        }

        public DbItemBuilder WithCity(string city)
        {
            var item = _context.Set<Item>().Local.Last();
            item.City = city;
            return this;
        }

        public DbItemBuilder WithStatus(ItemStatus status)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Status = status;
            return this;
        }

        public DbItemBuilder WithLostDateFrom(DateTime lostDateFrom)
        {
            var item = _context.Set<Item>().Local.Last();
            item.LostDateFrom = lostDateFrom;
            return this;
        }

        public DbItemBuilder WithLostDateTo(DateTime lostDateTo)
        {
            var item = _context.Set<Item>().Local.Last();
            item.LostDateTo = lostDateTo;
            return this;
        }

        public DbItemBuilder WithFoundDate(DateTime? foundDate)
        {
            var item = _context.Set<Item>().Local.Last();
            item.FoundDate = foundDate;
            return this;
        }

        public DbItemBuilder WithEventLocation(string eventLocation)
        {
            var item = _context.Set<Item>().Local.Last();
            item.EventLocation = eventLocation;
            return this;
        }

        public DbItemBuilder WithStorageLocation(string storageLocation)
        {
            var item = _context.Set<Item>().Local.Last();
            item.StorageLocation = storageLocation;
            return this;
        }

        public DbItemBuilder WithCountry(string country)
        {
            var item = _context.Set<Item>().Local.Last();
            item.Country = country;
            return this;
        }
    }

    public class DbItemDetailBuilder(DbBuilder builder) : DbItemBuilder(builder)
    {
        public DbItemDetailBuilder WithKey(string key)
        {
            var detail = _context.Set<ItemDetail>().Local.Last();
            detail.Key = key;
            return this;
        }

        public DbItemDetailBuilder WithValue(string value)
        {
            var detail = _context.Set<ItemDetail>().Local.Last();
            detail.Value = value;
            return this;
        }
    }
}