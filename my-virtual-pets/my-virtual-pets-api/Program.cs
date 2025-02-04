using ImageRecognition;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Repositories;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Services.Interfaces;
using PixelationTest;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    string memoryDbConnectionString = "Data Source=:memory:";
    var sqlConnection = new SqliteConnection(memoryDbConnectionString);
    sqlConnection.Open();
    builder.Services.AddDbContext<IDbContext, VPSqliteContext>(options => options.UseSqlite(sqlConnection));
    builder.Services.AddSwaggerGen();
    builder.Services.AddEndpointsApiExplorer();
}
else if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("ConnectionString__my_virtual_pets");
    Console.WriteLine(connectionString);
    builder.Services.AddDbContext<IDbContext, VPSqlServerContext>(options => options.UseSqlServer(connectionString));
}


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IStorageService, S3StorageService>();
builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddScoped<IPixelate, Pixelate>();
builder.Services.AddScoped<IRemoveBackgroundService, RemoveBackgroundService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
