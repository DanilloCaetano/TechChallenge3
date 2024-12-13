using Common.MessagingService;
using Domain.Region.Repository;
using Domain.Region.Service;
using Infraestructure.Context;
using Infraestructure.Repository.RegionRepository;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge1.DomainInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureRabbit(services);
            ConfigureRegions(services);

            return services;
        }

        private static void ConfigureRabbit(IServiceCollection services)
        {
            services.AddScoped<IRabbitMqService, RabbitMqService>();
        }

        public static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TechChallengeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        public static void ConfigureRegions(this IServiceCollection services)
        {
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IRegionService, RegionService>();
        }
    }
}
