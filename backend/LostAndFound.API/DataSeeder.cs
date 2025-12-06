using LostAndFound.Data.Plock;
using LostAndFound.Services;

namespace LostAndFound.API;

public interface IDataSeeder
{
    Task Seed();
}

public class DataSeeder(LostAndFoundDbContext context) : IDataSeeder
{
    public async Task Seed()
    {
        if (context.Items.Any())
        {
            return;
        }

        var dataSource = new DataSource();

        await foreach (var item in dataSource.GetItems())
        {
            var itemData = new Item
            {
                Name = item.Title ?? "",
                Description = item.Content ?? "Do odbioru w Biurze Rzeczy Znalezionych miasta Płock.",
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                StorageLocation = "Biuro Rzeczy Znalezionych, Płock",
                Details = item.ContentTypeFields.Select(x => new ItemDetail
                {
                    Key = x.NodeName ?? "",
                    Value = x.NodeValue ?? ""
                }).ToList()
            };

            await context.Items.AddAsync(itemData);
        }

        await context.SaveChangesAsync();
    }
}