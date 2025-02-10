using ImageRecognition;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Data;
using my_virtual_pets_api.HealthChecks;
using my_virtual_pets_api.Repositories;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Services.Interfaces;
using PixelationTest;
using System.Text;
using System.Text.Json.Serialization;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using my_virtual_pets_api.HealthChecks;


var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    string memoryDbConnectionString = "Data Source=:memory:;Pooling=true;";
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
    builder.Services.AddHealthChecks().AddCheck("Db-check", new SqlServerHealthCheck(connectionString),HealthStatus.Unhealthy,new string[] { "orderingdb" });
    builder.Services.AddDbContext<IDbContext, VPSqlServerContext>(options => options.UseSqlServer(connectionString));
}


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IStorageService, S3StorageService>();

builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddHttpClient<ImageRecognitionHealthCheck>();
builder.Services.AddHealthChecks().AddCheck<ImageRecognitionHealthCheck>("image-recognition-check");

builder.Services.AddScoped<IPixelateService, PixelateService>();

builder.Services.AddScoped<IRemoveBackgroundService, RemoveBackgroundService>();
builder.Services.AddHttpClient<RemoveBackgroundHealthCheck>();
builder.Services.AddHealthChecks().AddCheck<RemoveBackgroundHealthCheck>("remove-background-check");

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddMemoryCache();


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }
         )
    .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new Exception("Google Client ID could not be found.");;
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new Exception("Google Client Secret could not be found.");;
            options.CallbackPath = "/signin-google";
            options.Scope.Add("email");
            options.Scope.Add("profile");
        }
    )
    .AddJwtBearer("loginjwt", options =>
  {
options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = "my-virtual-pets.com",
             ValidAudience = "my-virtual-pets.com",
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:jwt:SecretKey"]))
         };
     });


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend",
        policy =>
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
