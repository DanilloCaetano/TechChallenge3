using Common.MessagingService;
using ContactConsumer;
using Domain.Region.Repository;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InsertWorker>();
builder.Services.AddDbContext<TechChallengeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddTransient<IRegionRepository, Infraestructure.Repository.RegionRepository.RegionRepository>();

var host = builder.Build();
host.Run();
