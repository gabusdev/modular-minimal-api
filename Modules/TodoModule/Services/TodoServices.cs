using Modules.DataModule.Entities;
using Modules.TodoModule.Models;
using Modules.MainModule.Services;

namespace modular.Modules.TodoModule.Services
{
    public class TodoServices
    {
        private readonly DefaultDbContext _context;
        private readonly IUserService _userService;
        private IHttpContextAccessor _httpContext;
        public TodoServices(DefaultDbContext context, IUserService userService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userService = userService;
            _httpContext = httpContext;
        }
        //     public async Task<IResult> Post(TodoDto newTodo)
        //     {
        //         _userService.
        //     }
        // }
    }
}