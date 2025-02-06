using System.Text;
using ImageRecognition;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using my_virtual_pets_api.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    string memoryDbConnectionString = "Data Source=:memory:";
    var sqlConnection = new SqliteConnection(memoryDbConnectionString);
    sqlConnection.Open();
    builder.Services.AddDbContext<IDbContext, VPSqliteContext>(options => options.UseSqlite(sqlConnection));
    builder.Services.AddHealthChecks().AddCheck("Db-check", new SqlConnectionHealthCheck(memoryDbConnectionString), HealthStatus.Unhealthy, new string[] { "orderingdb" });
    builder.Services.AddSwaggerGen();
    builder.Services.AddEndpointsApiExplorer();
    
}
else if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("ConnectionString__my_virtual_pets");
    Console.WriteLine(connectionString);
    builder.Services.AddHealthChecks().AddCheck("Db-check", new SqlServerHealthCheck(connectionString),HealthStatus.Unhealthy,new string[] { "orderingdb" });
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

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "my-virtual-pets.com",
            ValidAudience = "my-virtual-pets.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b3f7e9d8f6a4b9c2d1a6c8e0e0f9b1a3")) // test token remove later
        };
    });
// need to configure Google OAuth here 

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend",
        policy  =>
        {
            policy.WithOrigins("https://localhost:7247", "http://localhost:5092");
        });
    options.AddPolicy("AllowAll", 
        builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/health");


app.MapControllers();

app.Run();
