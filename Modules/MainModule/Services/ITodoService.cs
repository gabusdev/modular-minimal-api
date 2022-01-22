using Modules.MainModule.Models;

namespace Modules.MainModule.Services
{
    public interface ITodoService
    {
        Task<IResult> Add(CreateTodoDto todoDto, HttpContext httpContext);
        Task<IResult> GetAll(HttpContext cont);
        Task<IResult> Delete(string id, HttpContext cont);
    }
}