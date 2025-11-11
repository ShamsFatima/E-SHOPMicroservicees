using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using Marten;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health");
app.Run();
