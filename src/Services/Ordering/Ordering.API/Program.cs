using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAppplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseAPiServices();
if (app.Environment.IsDevelopment())
{
   await app.Services.InitialiseDatabaseAsync();
}
app.Run();
