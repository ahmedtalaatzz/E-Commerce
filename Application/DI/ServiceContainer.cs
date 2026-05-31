using Application.MapperConfig;
using Application.Service_Contract;
using Application.Services;
using e_commerce.core.ServiceContracts;
using e_commerce.core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    /// <summary>
    /// Application layer dependency injection configuration
    /// </summary>
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IFileService, FileService>();

            // Register AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappConfig>();
            });

            return services;
        }
    }
}