using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Modules.AuthenticationUtilsModule.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        public AuthenticationService(IConfiguration config)
        {
            _config = config;
        }
        public string MakeHash(string text)
        {
            using (var hmac = new HMACSHA512())
            {
                var textByte = System.Text.Encoding.UTF8.GetBytes(text);
                string hex = String.Empty;
                byte[] salt = hmac.Key;
                byte[] hash = hmac.ComputeHash(textByte);

                hex += Convert.ToHexString(salt);
                hex += ".";
                hex += Convert.ToHexString(hash);
                return hex;
            }
        }
        public string GenerateToken(Dictionary<string, string> data)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature);
            var encryption = new EncryptingCredentials(
                secret,
                SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = new List<Claim>();
            foreach (var item in data)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                new ClaimsIdentity(claims),
                DateTime.Now,
                DateTime.Now.AddMinutes(15),
                DateTime.Now,
                credentials,
                encryption
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool VerifyHash(string pass, string hashedPassword)
        {
            (byte[] passSalt, byte[] passHash) =
                (
                    Convert.FromHexString(hashedPassword.Split(".")[0]),
                    Convert.FromHexString(hashedPassword.Split(".")[1])
                );
            using (var hmac = new HMACSHA512(passSalt))
            {
                var textByte = System.Text.Encoding.UTF8.GetBytes(pass);
                byte[] hash = hmac.ComputeHash(textByte);
                return hash.SequenceEqual(passHash);
            }
        }
        public bool VerifyHash(string pass, string hashedPassword, string salt)
        {
            byte[] passSalt = Convert.FromHexString(salt);
            byte[] passHash = Convert.FromHexString(hashedPassword);
            using (var hmac = new HMACSHA512(passSalt))
            {
                var textByte = System.Text.Encoding.UTF8.GetBytes(pass);
                byte[] hash = hmac.ComputeHash(textByte);
                return hash.SequenceEqual(passHash);
            }
        }

    }
}