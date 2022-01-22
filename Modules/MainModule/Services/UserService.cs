// using Microsoft.AspNetCore.Mvc;
using Modules.DataModule.Entities;
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
            List<User> users = await _context.Users.Include(u => u.Roles).Include(u => u.Todos).AsSplitQuery().ToListAsync();
            List<UserDto> viewUsers = new List<UserDto>();
            foreach (var item in users)
            {
                viewUsers.Add(MakeUserViewModel(item));
            }
            return Results.Ok(viewUsers);
        }
        public async Task<IResult> Login(LoginDto userDto)
        {
            User? user = await _context.Users.Include(u => u.Roles).AsSplitQuery().FirstOrDefaultAsync(u =>
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
        public async Task<IResult> Register(RegisterDto userRegist)
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
                UserDto currentUser = await MakeUserViewModel(id);
                return currentUser is null ? Results.BadRequest() : Results.Ok(currentUser);
            }
            return Results.BadRequest();
        }
        public async Task<IResult> Put(RegisterDto userRegist, HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string id = identity.FindFirst(ClaimTypes.Sid)!.Value;
                User? currentUser = await GetCurrent(id);
                if (currentUser is not null)
                {
                    currentUser.Username = userRegist.UserName;
                    currentUser.Mail = userRegist.Mail;
                    currentUser.Pass = _authService.MakeHash(userRegist.Password);
                    _context.Entry(currentUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return await Login(new LoginDto
                    {
                        UserOrMail = userRegist.UserName,
                        Password = userRegist.Password
                    });
                }
                return Results.NotFound();
            }
            return Results.BadRequest();
        }
        private async Task<User?> GetCurrent(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        private async Task<UserDto> MakeUserViewModel(string id)
        {
            User user = await _context.Users.Include("Todos").Include("Roles").AsSplitQuery().FirstAsync(u => u.Id == id);
            return MakeUserViewModel(user);
        }
        private UserDto MakeUserViewModel(User user)
        {
            List<string> viewRoles = new List<string>();
            foreach (var item in user.Roles)
            {
                viewRoles.Add(item.Name);
            }
            List<TodoDto> viewTodos = new List<TodoDto>();
            foreach (var item in user.Todos)
            {
                viewTodos.Add(new TodoDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsDone = item.IsDone
                }
                );
            }
            UserDto viewUser = new UserDto
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