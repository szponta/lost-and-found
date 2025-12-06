using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public static class DbFactory
{
    public static LostAndFoundDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<LostAndFoundDbContext>()
            .UseInMemoryDatabase("InMemoryLostAndFoundDb")
            .Options;

        var context = new LostAndFoundDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}