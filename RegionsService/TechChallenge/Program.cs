using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Reflection;
using System.Text.Json.Serialization;
using TechChallenge1.DomainInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tech Challenge",
        Description = "Tech Challenge FIAP",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Transient);

builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

var counter = Metrics.CreateCounter("fiaptc", "Counts requests to the fiaptc",
    new CounterConfiguration
    {
        LabelNames = new[] { "method", "endpoint", "status_code" }
    });

var requestCount = Metrics.CreateCounter(
            "httpclient_requests_received_total",
            "Count of HTTP requests that have been completed by an HttpClient.",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "host", "status_code" }
            });

var requestDuration = Metrics.CreateHistogram(
            "httpclient_request_duration_seconds",
            "Duration histogram of HTTP requests performed by an HttpClient.",
            new HistogramConfiguration
            {
                // 1 ms to 32K ms buckets
                Buckets = Histogram.ExponentialBuckets(0.001, 2, 16),
                LabelNames = new[] { "method", "host", "status_code" }
            });

app.Use((context, next) =>
{
    counter.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Inc();
    requestCount.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Inc();
    requestDuration.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Publish();
    return next();
});

app.UseMetricServer(settings => settings.EnableOpenMetrics = false);
app.UseHttpMetrics();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHttpMetrics(options =>

{
    options.RequestDuration.Enabled = true;
    options.RequestCount.Enabled = true;

});
app.MapHealthChecks("/healthz");

app.MapControllers();

app.Run();
