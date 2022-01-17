// using Microsoft.AspNetCore.Mvc;
using Modules.MainModule.Models;
using Modules.AuthenticationUtilsModule.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Modules.MainModule.Services
{

    public class UserService : IUserService
    {
        private DefaultDbContext _context;
        private IAuthenticationService _authService;

        public UserService(DefaultDbContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _context.Users.ToListAsync());
        }
        public async Task<IResult> Login(UserDto userDto)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u =>
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
            await _context.Users.AddAsync(newUser);
            Role userRole = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User"))!;
            // await _context.UserRols.AddAsync(userRol);
            await _context.SaveChangesAsync();
            newUser.Roles.Add(userRole);
            await _context.SaveChangesAsync();


            return Results.Created($"/User/{newUser.Id}", newUser);
        }
        public async Task<IResult> Info(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                User? currentUser = await GetCurrent(identity.FindFirst(ClaimTypes.Sid)!.Value);
                return currentUser is null ? Results.BadRequest() : Results.Ok(currentUser);
            }
            return Results.BadRequest();
        }

        private async Task<User?> GetCurrent(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}