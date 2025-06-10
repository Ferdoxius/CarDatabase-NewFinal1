using System.Text;
using Serilog;
using FluentValidation;
using FluentValidation.AspNetCore;
using CarDatabase.BL;
using CarDatabase.DL;
using CarDatabase.Models;
using CarDatabase.BL.Kafka;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

var mongoConnectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
    ?? throw new InvalidOperationException("MongoDB connection string is missing.");
var mongoDatabaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName")
    ?? throw new InvalidOperationException("MongoDB database name is missing.");

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddSingleton<ICarService, CarService>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CarValidator>();

builder.Services.AddSingleton<KafkaCarProducerService>();
//builder.Services.AddSingleton<KafkaCarConsumerService>();
//builder.Services.AddHostedService(sp => sp.GetRequiredService<KafkaCarConsumerService>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Car Database API",
        Version = "v1",
        Description = "API за управление на автомобили."
    });
    c.AddServer(new OpenApiServer { Url = "https://localhost:7070" });
});

builder.Services.AddHealthChecks()
    .AddMongoDb(sp => sp.GetRequiredService<IMongoClient>(),
        name: "mongodb",
        timeout: TimeSpan.FromSeconds(3),
        failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Database API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

builder.Services.AddSingleton<KafkaCarConsumerService>();


