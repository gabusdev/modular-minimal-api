namespace Modules.AuthenticationUtilsModule.Services
{
    public interface IAuthenticationService
    {
        string MakeHash(string text);
        string GenerateToken(Dictionary<string, string> data);
        bool VerifyHash(string pass, string hashedPassword);
        bool VerifyHash(string pass, string hashedPassword, string salt);
    }
}