using Microsoft.EntityFrameworkCore;
using Modules.DataModule.Entities;

namespace Modules.DataModule
{
    public class ClassName : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection service, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                "server=localhost;database=TestDB;user=root;password=1234";
            var MySqlVersion = new MySqlServerVersion(new Version(8, 0, 27));

            service.AddDbContext<DefaultDbContext>(opt =>
                opt.UseMySql(
                    connectionString,
                    MySqlVersion
            ));
            return service;
        }
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }
    }
}