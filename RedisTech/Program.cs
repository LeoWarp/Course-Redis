using RedisTech.Application.DependencyInjection;
using RedisTech.DAL.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOutputCache();  // добавляем сервисы

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.UseOutputCache();       // добавляем OutputCacheMiddleware
//app.UseMiddleware<SettingsCacheHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

	
app.MapGet("/", () =>
{
    
}).CacheOutput();  // применяем кэширование

app.Run();