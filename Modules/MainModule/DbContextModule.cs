using Modules.MainModule.Models;
using Microsoft.EntityFrameworkCore;
namespace Modules.MainModule
{
    public class DbContextModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<DefaultDbContext>(opt =>
                opt.UseMySql(
                    config.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 27))
            )
        );
            return service;
        }
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }
    }
}