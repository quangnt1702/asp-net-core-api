using Microsoft.Extensions.DependencyInjection;
using Second_Project.Services;

namespace Second_Project.Helpers
{
    public static class AppServiceCollection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAuthenService, AuthenService>();
            return services;
        }
    }
}