using Carter;
using Shared.Enums.Role;
using Shared.Extensions;
using Todo;
using User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var todoAssembly = typeof(TodoModule).Assembly;
var userAssembly = typeof(UserModule).Assembly;

var assemblies = new[] { todoAssembly, userAssembly };

builder.Services.AddCarterWithAssemblies(assemblies);
builder.Services.AddMediatRWithAssemblies(assemblies);

builder.Services.AddTodoModule(builder.Configuration);
builder.Services.AddUserModule(builder.Configuration);

//builder.Services.AddCarter();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis:Dev");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.MapGet("/", () =>
{
    return "Hello World!";
})
    .RequireAuthorization(policy =>
    {
        policy.RequireRole(RoleType.Admin.ToString());
    });

app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
