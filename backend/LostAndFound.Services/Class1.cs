using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<MyEntity> MyEntities { get; set; } = null!;
}

public class MyEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
}