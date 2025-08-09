using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationFarmerShop.Middlewares;
using WebApplicationFarmerShop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IIpPolicyStore,MemoryCacheIpPolicyStore >();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<IBuyService, BuyService>();
builder.Services.AddInMemoryRateLimiting(); 

builder.Services.Configure<ClientRateLimitOptions>(options =>
{
   options.EnableEndpointRateLimiting = true;
   options.StackBlockedRequests = false;
   options.HttpStatusCode = 429; // Too Many Requests
   options.RealIpHeader = "X-Real-IP"; // Use this header to get the real IP address of the client
   options.ClientIdHeader = "X-ClientId"; 
   options.GeneralRules = new List<RateLimitRule>
   {
       new RateLimitRule
       {
           Endpoint = "*",
           Period = "1m",
           Limit = 1 // Allow 1 request per minute
       }
   };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ClientFarmRateLimitMiddleware>();

app.UseHttpsRedirection();



app.MapPost("/corns/buy", async ([FromServices] IBuyService buyService) =>
{
    var result = await buyService.BuyAsync("corn001", 1);
    if (result.Success)
    {
        return Results.Ok(result.Message);
    }
    else
    {
        return Results.Problem(result.Message);
    }
}).Produces(StatusCodes.Status200OK)
.WithName("BuyCorn")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
