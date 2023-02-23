using Microsoft.AspNetCore.DataProtection;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConnection = builder.Configuration.GetConnectionString("Redis")?.ToString();
var redisKey = builder.Configuration.GetSection("Redis:Key").Value;

var redis = ConnectionMultiplexer.Connect(redisConnection!);

// Redis 데이터 접근 보호 Key 설정
builder.Services
    .AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, redisKey);

builder.Services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = redisConnection;
});

builder.Services.AddSession(o => {
    o.Cookie.Name = "session";
    o.Cookie.HttpOnly = true;
    o.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
