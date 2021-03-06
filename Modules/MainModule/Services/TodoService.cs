using Modules.DataModule.Entities;
using Modules.MainModule.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Modules.MainModule.Services
{
    public class TodoService : ITodoService
    {
        private readonly DefaultDbContext _context;

        public TodoService(DefaultDbContext context)
        {
            _context = context;
        }
        public async Task<IResult> Add(CreateTodoDto todoDto, HttpContext httpContext)
        {
            string userId = (await GetCurrentUser(httpContext)).Id;

            Todo newTodo = new Todo
            {
                Id = Guid.NewGuid().ToString(),
                Name = todoDto.Name,
                IsDone = todoDto.IsDone,
                UserId = userId
            };
            await _context.Todos.AddAsync(newTodo);
            await _context.SaveChangesAsync();

            return Results.Created("/User/todos", todoDto);
        }
        public async Task<IResult> GetAll(HttpContext cont)
        {
            User user = await GetCurrentUser(cont);
            List<TodoDto> userTodos = new List<TodoDto>();
            foreach (var item in user.Todos)
            {
                userTodos.Add(new TodoDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsDone = item.IsDone
                });
            }
            return Results.Ok(userTodos);
        }
        public async Task<IResult> Delete(string id, HttpContext cont)
        {
            var todoItem = await _context.Todos.FindAsync(id);
            if (todoItem == null)
            {
                return Results.NotFound();
            }
            if (todoItem.UserId != (await GetCurrentUser(cont)).Id)
            {
                return Results.BadRequest();
            }
            _context.Todos.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Results.NoContent();
        }
        private async Task<User> GetCurrentUser(HttpContext cont)
        {
            var identity = (cont.User.Identity as ClaimsIdentity)!;
            // if (identity == null)
            // {
            //     return null;
            // }
            string userId = identity.FindFirst(ClaimTypes.Sid)!.Value;
            return await _context.Users.Include(u => u.Todos).AsSplitQuery().FirstAsync(u => u.Id == userId);
        }
    }
}