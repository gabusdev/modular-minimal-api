namespace Modules.MainModule
{
    public class ClassName : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection service, IConfiguration config)
        {
            return service;
        }
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }
    }
}