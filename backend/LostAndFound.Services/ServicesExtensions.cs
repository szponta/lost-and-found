using Microsoft.Extensions.DependencyInjection;

namespace LostAndFound.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddLostAndFoundServices(this IServiceCollection services)
    {
        services.AddScoped<IGetItemsHandler, GetItemsHandler>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        return services;
    }
}