using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    string memoryDbConnectionString = "Data Source=:memory:";
    var sqlConnection = new SqliteConnection(memoryDbConnectionString);
    sqlConnection.Open();
    builder.Services.AddDbContext<VPSqliteContext>(options => options.UseSqlite(sqlConnection));
    builder.Services.AddSwaggerGen();
}
else if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("ConnectionString__my_virtual_pets");
    builder.Services.AddDbContext<VPSqlServerContext>(options => options.UseSqlServer(connectionString)); 
}


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
