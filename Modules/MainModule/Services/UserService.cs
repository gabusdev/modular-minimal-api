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
                    foreach (var item in user.UserRols)
                    {
                        data.Add(ClaimTypes.Role, item.Roles.Name);
                    }
                    data.Add(ClaimTypes.Name, user.Username);
                    data.Add(ClaimTypes.Email, user.Mail);
                    data.Add(ClaimTypes.Sid, user.Id);

                    return Results.Ok(_authService.GenerateToken(data));
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
            UserRol userRol = new UserRol
            {
                UserId = newUser.Id,
                RolesId = 2
            };
            await _context.Users.AddAsync(newUser);
            await _context.UserRols.AddAsync(userRol);
            await _context.SaveChangesAsync();

            return Results.Created($"/User/{newUser.Id}", newUser);
        }
    }
}