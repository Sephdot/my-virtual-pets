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
