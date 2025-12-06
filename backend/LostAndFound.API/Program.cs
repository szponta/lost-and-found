using LostAndFound.API;
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

builder.Services.AddLostAndFoundServices();

builder.Services.AddScoped<IDataSeeder, DataSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await dataSeeder.Seed();
}

app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/api/v1/test", () => new { message = "hello world" }).WithName("Test");

app.MapGet("/api/v1/items",
    (IGetItemsHandler handler, [FromQuery] int take = 10, int skip = 0,
            string search = "",
            DateTime? foundDateFrom = null) =>
        handler.HandleAsync(take, skip, search, foundDateFrom));

Log.Information("Application running.");

app.Run();