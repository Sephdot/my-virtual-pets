using ImageRecognition;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using my_virtual_pets_api.Repositories;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using PixelationTest;
using Microsoft.IdentityModel.Tokens; 

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

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "my_virtual_pets_api";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.LoginPath = "/api/user/login"; // This path will need to serve a login web page - front end routes? 
        options.LogoutPath = "/api/user/logout"; // This path will need to serve a logout web page - 
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/api/user/forbidden";
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


// app.UseCors("Frontend");
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
