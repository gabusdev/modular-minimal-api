// using Microsoft.AspNetCore.Mvc;
using Modules.MainModule.Entities;
using Modules.MainModule.Models;
using Modules.AuthenticationUtilsModule.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Modules.MainModule.Services
{

    public class UserService : IUserService
    {
        private readonly DefaultDbContext _context;
        private readonly IAuthenticationService _authService;

        public UserService(DefaultDbContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<IResult> GetAll()
        {
            List<User> users = await _context.Users.Include(u => u.Roles).Include(u => u.Todos).ToListAsync();
            List<UserViewModel> viewUsers = new List<UserViewModel>();
            foreach (var item in users)
            {
                viewUsers.Add(MakeUserViewModel(item));
            }
            return Results.Ok(viewUsers);
        }
        public async Task<IResult> Login(UserDto userDto)
        {
            User? user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u =>
                u.Username.ToLower().Equals(userDto.UserOrMail.ToLower()) ||
                u.Mail.ToLower().Equals(userDto.UserOrMail));
            if (user is not null)
            {
                if (_authService.VerifyHash(userDto.Password, user.Pass))
                {
                    var data = new Dictionary<string, string>();
                    foreach (var item in user.Roles)
                    {
                        data.Add(ClaimTypes.Role, item.Name);
                    }
                    data.Add(ClaimTypes.Name, user.Username);
                    data.Add(ClaimTypes.Email, user.Mail);
                    data.Add(ClaimTypes.Sid, user.Id);

                    return Results.Text(_authService.GenerateToken(data));
                }
                else
                {
                    return Results.BadRequest("Contraseña incorrecta");
                }
            }
            return Results.NotFound("Usuario no encontrado");
        }
        public async Task<IResult> Register(UserRegister userRegist)
        {
            var id = Guid.NewGuid().ToString();
            User newUser = new User
            {
                Id = id,
                Username = userRegist.UserName,
                Pass = _authService.MakeHash(userRegist.Password),
                Mail = userRegist.Mail

            };
            Role userRole = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User"))!;
            newUser.Roles.Add(userRole);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return Results.Ok(MakeUserViewModel(newUser));
        }
        public async Task<IResult> Info(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string id = identity.FindFirst(ClaimTypes.Sid)!.Value;
                UserViewModel currentUser = await MakeUserViewModel(id);
                return currentUser is null ? Results.BadRequest() : Results.Ok(currentUser);
            }
            return Results.BadRequest();
        }
        private async Task<User?> GetCurrent(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        private async Task<UserViewModel> MakeUserViewModel(string id)
        {
            User user = await _context.Users.Include("Todos").Include("Roles").FirstAsync(u => u.Id == id);
            return MakeUserViewModel(user);
        }
        private UserViewModel MakeUserViewModel(User user)
        {
            List<RoleViewModel> viewRoles = new List<RoleViewModel>();
            foreach (var item in user.Roles)
            {
                viewRoles.Add(new RoleViewModel
                {
                    Name = item.Name
                }
                );
            }
            List<TodoViewModel> viewTodos = new List<TodoViewModel>();
            foreach (var item in user.Todos)
            {
                viewTodos.Add(new TodoViewModel
                {
                    Name = item.Name,
                    IsDone = item.IsDone
                }
                );
            }
            UserViewModel viewUser = new UserViewModel
            {
                Username = user.Username,
                Mail = user.Mail,
                Roles = viewRoles,
                Todos = viewTodos
            };
            return viewUser;
        }
    }
}