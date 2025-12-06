using Microsoft.Extensions.DependencyInjection;

namespace LostAndFound.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddLostAndFoundServices(this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
        services.AddScoped<IGetItemsHandler, GetItemsHandler>();
        services.AddScoped<IGetSingleItemHandler, GetSingleItemHandler>();
        services.AddScoped<IPostItemHandler, PostItemHandler>();
        return services;
    }
}