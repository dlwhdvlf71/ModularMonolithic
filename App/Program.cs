using Carter;
using Email;
using Shared.Enums.Role;
using Shared.Extensions;
using Shared.Messaging.Extensions;
using Todo;
using User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var todoAssembly = typeof(TodoModule).Assembly;
var userAssembly = typeof(UserModule).Assembly;
var emailAssembly = typeof(EmailModule).Assembly;

var assemblies = new[] { todoAssembly, userAssembly, emailAssembly };

builder.Services.AddCarterWithAssemblies(assemblies);
builder.Services.AddMediatRWithAssemblies(assemblies);
builder.Services.AddMassTransitWithAssemblies(builder.Configuration, assemblies);

builder.Services.AddTodoModule(builder.Configuration);
builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddEmailModule(builder.Configuration);

//builder.Services.AddCarter();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis:Dev");
});

//builder.Services.AddMassTransitWithAssemblies(assemblies);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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