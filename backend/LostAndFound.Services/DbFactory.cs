using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public static class DbFactory
{
    public static LostAndFoundDbContext GetInMemoryDbContext()
    {
        var guid = Guid.NewGuid().ToString();

        var options = new DbContextOptionsBuilder<LostAndFoundDbContext>()
            .UseInMemoryDatabase(guid)
            .Options;

        var context = new LostAndFoundDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}