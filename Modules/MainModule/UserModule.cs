using Modules.MainModule.Models;
using Modules.AuthenticationUtilsModule.Services;
using Modules.MainModule.Services;
using Microsoft.AspNetCore.Mvc;
namespace Modules.MainModule
{
    public class UserModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection service, IConfiguration config)
        {
            service.AddTransient<IAuthenticationService, AuthenticationService>();
            service.AddScoped<IUserService, UserService>();
            return service;
        }
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/v1/User", (IUserService service) =>
                service.GetAll())
                .RequireAuthorization("Admin")
                .Produces(StatusCodes.Status200OK)
                .WithTags("User");
            endpoints.MapGet("/api/v1/User/info", (IUserService service, HttpContext httpContext) =>
                service.Info(httpContext));
            endpoints.MapPost("/api/v1/User/login", (UserDto userDto, IUserService service) =>
                service.Login(userDto))
                .AllowAnonymous()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags("User");
            endpoints.MapPost("/api/v1/User/register", (UserRegister userRegist, IUserService service) =>
                service.Register(userRegist))
                .AllowAnonymous()
                .Produces(StatusCodes.Status201Created)
                .WithTags("User");
            // endpoints.MapGet("/api/v1/User/{id}", (IUserService service) => service.Get);
            // endpoints.MapPut("/api/v1/User/{id}", (IUserService service) => service.Put);
            // endpoints.MapDelete("/api/v1/User/{id}", (IUserService service) => service.Delete);

            return endpoints;
        }
    }
}