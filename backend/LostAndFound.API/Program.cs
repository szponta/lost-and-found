using LostAndFound.Data.Plock;
using LostAndFound.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables()
    .Build();

var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(configuration);

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, true));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LostAndFoundDbContext>(o => o.UseInMemoryDatabase("MyMemoryDb"));

builder.Services.AddServices();

var app = builder.Build();

// add data from datasource to in-memory database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LostAndFoundDbContext>();
    await DataSeeder.Seed(dbContext);
}


app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/api/v1/test", () => new { message = "hello world" }).WithName("Test");

app.MapGet("/api/v1/items",
    (IGetItemsHandler handler, [FromQuery] int take = 10, int skip = 0) => handler.HandleAsync(take, skip));

Log.Information("Application running.");

app.Run();

public static class DataSeeder
{
    public static async Task Seed(LostAndFoundDbContext context)
    {
        if (context.Items.Any()) return;

        var dataSource = new DataSource();

        // async foreach
        await foreach (var item in dataSource.GetItems())
        {
            var itemData = new Item
            {
                Title = item.Title ?? "",
                Description = item.Content ?? "Do odbioru w Biurze Rzeczy Znalezionych miasta Płock.",
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
                // Details = item.ContentTypeFields.Select()
            };

            await context.Items.AddAsync(itemData);
        }

        await context.SaveChangesAsync();
    }
}