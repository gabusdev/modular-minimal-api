namespace Modules.MainModule.Models
{
    public class LoginDto
    {
        public string UserOrMail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}