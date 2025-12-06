using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services;

public class LostAndFoundDbContext(DbContextOptions<LostAndFoundDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemDetail> ItemDetails { get; set; } = null!;
}