using StackExchange.Redis;
using Store.BasketService.Commands;
using Store.BasketService.Infrastructure.Repository;
using Store.BasketService.Infrastructure.Repository.Configuration;
using Store.BasketService.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
     );
});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var connectionString =
        builder.Configuration["Redis:ConnectionString"]!;

    return ConnectionMultiplexer.Connect(connectionString);
});


builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
builder.Services.AddTransient<IBasketProvider, BasketProvider>();

builder.Services.AddSingleton<IAddProductToBasketCommand, AddProductToBasketCommand>();
builder.Services.AddSingleton<IRemoveProductFromBasketCommand, RemoveProductFromBasketCommand>();

// Add connections to azure sql storage

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
