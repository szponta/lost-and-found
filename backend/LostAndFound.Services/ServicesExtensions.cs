using Microsoft.Extensions.DependencyInjection;

namespace LostAndFound.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGetItemsHandler, GetItemsHandler>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        return services;
    }
}