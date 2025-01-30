using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using my_virtual_pets_api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


string memoryDbConnectionString = "Data Source=:memory:";

var sqlConnection = new SqliteConnection(memoryDbConnectionString);

sqlConnection.Open();

builder.Services.AddDbContext<VPSqliteContext>(options => options.UseSqlite(sqlConnection));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
