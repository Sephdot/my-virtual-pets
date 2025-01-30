using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace my_virtual_pets_api.Data
{
    public class VPSqlServerFactory : IDesignTimeDbContextFactory<VPSqlServerContext>
    {
        public VPSqlServerContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString__my_virtual_pets");
            var optionsBuilder = new DbContextOptionsBuilder<VPSqlServerContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new VPSqlServerContext(optionsBuilder.Options);

        }
    }
}
