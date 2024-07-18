using MeteoriteApp.Server.BLL;
using MeteoriteApp.Server.BLL.Models;
using MeteoriteApp.Server.BLL.Services;
using MeteoriteApp.Server.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextFactory<MeteoriteContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddSingleton<MeteoriteContext>();
builder.Services.AddSingleton<IFetchMeteoriteClientService, FetchMeteoriteClientService>();
builder.Services.AddSingleton<IMeteoriteService, MeteoriteService>();

builder.Services.AddHttpClient();

builder.Services.AddMemoryCache();

builder.Services.Configure<MeteoriteFetchOptions>(builder.Configuration.GetSection("MeteoriteFetch"));

builder.Services.AddHostedService<MeteoriteFetchService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
     .AllowAnyMethod()
     .AllowAnyHeader()
     .WithOrigins("https://localhost:44305", "https://localhost:5173", "http://localhost:5173")
     .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
