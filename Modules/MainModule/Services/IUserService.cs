using Modules.MainModule.Models;
namespace Modules.MainModule.Services
{
    public interface IUserService
    {
        Task<IResult> GetAll();
        Task<IResult> Login(LoginDto userDto);
        Task<IResult> Register(RegisterDto user);
        Task<IResult> Info(HttpContext httpContext);
        Task<IResult> Put(RegisterDto userRegist, HttpContext httpContext);
    }
}