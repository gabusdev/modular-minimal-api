using Modules.MainModule.Models;
using Modules.AuthenticationUtilsModule.Services;
using Modules.MainModule.Services;
namespace Modules.MainModule
{
    public class UserModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection service, IConfiguration config)
        {
            service.AddTransient<IAuthenticationService, AuthenticationService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ITodoService, TodoService>();
            return service;
        }
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            // GET All Users (only Admin)
            endpoints.MapGet("/api/v1/User", (IUserService service) =>
                service.GetAll())
                .RequireAuthorization("Admin")
                .Produces(StatusCodes.Status200OK)
                .WithTags("User");
            // GET info of Current User
            endpoints.MapGet("/api/v1/User/info", (IUserService service, HttpContext httpContext) =>
                service.Info(httpContext))
                .Produces<UserDto>(StatusCodes.Status200OK)
                .WithTags("User");
            // POST For login and getting token
            endpoints.MapPost("/api/v1/User/login", (LoginDto userDto, IUserService service) =>
                service.Login(userDto))
                .AllowAnonymous()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags("User");
            // POST To register new User
            endpoints.MapPost("/api/v1/User/register", (RegisterDto userRegist, IUserService service) =>
                service.Register(userRegist))
                .AllowAnonymous()
                .Produces(StatusCodes.Status201Created)
                .WithTags("User");
            // PUT To updae current User
            endpoints.MapPut("/api/v1/User", (RegisterDto userRegist, HttpContext httpContext, IUserService service) =>
                service.Put(userRegist, httpContext))
                .WithTags("User");
            // POST To create new todo for current user
            endpoints.MapPost("/api/v1/User/todo", (CreateTodoDto todoDto, HttpContext httpContext, ITodoService service) =>
                service.Add(todoDto, httpContext)
            ).WithTags("Todos");
            // GET All todos of current user
            endpoints.MapGet("/api/v1/User/todo", (HttpContext cont, ITodoService service) =>
                service.GetAll(cont)
            ).WithTags("Todos");
            // DELETE Selected todo
            endpoints.MapDelete("/api/v1/User/todo/{id}", (string id, HttpContext cont, ITodoService service) =>
                service.Delete(id, cont)
            ).WithTags("Todos");

            // endpoints.MapGet("/api/v1/User/{id}", (IUserService service) => service.Get);
            // endpoints.MapDelete("/api/v1/User/{id}", (IUserService service) => service.Delete);

            return endpoints;
        }
    }
}