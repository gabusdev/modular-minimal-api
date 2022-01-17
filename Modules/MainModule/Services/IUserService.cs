using Modules.MainModule.Models;
namespace Modules.MainModule.Services
{
    public interface IUserService
    {
        Task<IResult> GetAll();
        Task<IResult> Login(UserDto userDto);
        Task<IResult> Register(UserRegister user);
        Task<IResult> Info(HttpContext httpContext);
    }
}