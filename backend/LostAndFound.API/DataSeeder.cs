using LostAndFound.Data.Olsztyn;
using LostAndFound.Data.Plock;
using LostAndFound.Services;

namespace LostAndFound.API;

public interface IDataSeeder
{
    Task Seed();
}

public class DataSeeder(LostAndFoundDbContext context) : IDataSeeder
{
    private const string FoundDateKey = "Data przekazania do biura";

    public async Task Seed()
    {
        if (context.Items.Any())
        {
            return;
        }

        var dataSource = new DataSourcePlock();

        await foreach (var item in dataSource.GetItems())
        {
            var foundDate = GetFoundDateFrom(item);

            var itemData = new Item
            {
                Name = item.Title ?? "",
                Description = item.Content ?? "Do odbioru w Biurze Rzeczy Znalezionych miasta Płock.",
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                FoundDate = foundDate,
                City = "Płock",
                Country = "Polska",
                StorageLocation = "Biuro Rzeczy Znalezionych, Płock",


                Details = item.ContentTypeFields
                    .Where(x => x.NodeName != FoundDateKey)
                    .Select(x => new ItemDetail
                    {
                        Key = x.NodeName ?? "",
                        Value = x.NodeValue ?? ""
                    }).ToList()
            };

            await context.Items.AddAsync(itemData);
        }

        var dataSource2 = new DataSourceOlsztyn();

        await foreach (var item in dataSource2.GetItems())
        {
            var itemData = new Item
            {
                Name = item.Name ?? "",
                CreatedAt = item.ReceivedAt,
                UpdatedAt = item.ReceivedAt,
                FoundDate = item.ReceivedAt,
                City = "Olsztyn",
                Country = "Polska",
                StorageLocation = "Biuro Rzeczy Znalezionych, Olsztyn"
            };

            await context.Items.AddAsync(itemData);
        }

        await context.SaveChangesAsync();
    }

    private static DateTime? GetFoundDateFrom(PlockDataItem item)
    {
        var foundDate = item.ContentTypeFields.FirstOrDefault(x => x.NodeName == FoundDateKey);

        if (foundDate == null || string.IsNullOrEmpty(foundDate.NodeValue))
        {
            return null;
        }

        return DateTime.Parse(foundDate.NodeValue);
    }
}