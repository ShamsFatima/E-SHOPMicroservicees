using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Marten;
using Marten.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly=typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);    
}) 
.UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository,CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddGrpcClient<Discount.Grpc.Protos.DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapCarter();
app.UseExceptionHandler(options =>
{

});
app.UseHealthChecks("/health",new HealthCheckOptions { ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse});

app.Run();
