using Common.MessagingService;
using Domain.Contact.Repository;
using Domain.Contact.Service;
using Infraestructure.Context;
using Infraestructure.Repository.ContactsRepository;
using Integration;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge1.DomainInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureContacts(services);
            ConfigureRabbit(services);
            ConfigureIntegration(services);

            return services;
        }

        private static void ConfigureIntegration(IServiceCollection services)
        {
            services.AddScoped<IIntegrationService, IntegrationService>();
        }

        private static void ConfigureRabbit(IServiceCollection services)
        {
            services.AddScoped<IRabbitMqService, RabbitMqService>();
        }

        public static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TechChallengeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        public static void ConfigureContacts(this IServiceCollection services)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactService, ContactService>();
        }
    }
}
