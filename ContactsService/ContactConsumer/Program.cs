using Common.MessagingService;
using ContactConsumer;
using Domain.Contact.Repository;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InsertWorker>();

builder.Services.AddDbContext<TechChallengeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddTransient<IContactRepository, Infraestructure.Repository.ContactsRepository.ContactRepository>();

var host = builder.Build();
host.Run();
