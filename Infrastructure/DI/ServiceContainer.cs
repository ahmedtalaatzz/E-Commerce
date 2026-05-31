using Domain.IRepositries;
using Infrastructure.DataSeeding;
using Infrastructure.Repositries;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    /// <summary>
    /// Infrastructure layer dependency injection configuration
    /// </summary>
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();

            // Register data seeder
            services.AddScoped<IDataSeeder, DatabaseSeeder>();

            return services;
        }
    }
}
