using UserManagement.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagement.API.Features.Auth
{
    public interface IJWTGenerator
    {
        string GenerateToken(ApplicationUser user);
    }


    /// <summary>
    /// Uses symmetric key encryption to generate JWT tokens.
    /// This is only for development since no need to generate certs for now.
    /// </summary>
    public class SymmetricJWTGenerator : IJWTGenerator
    {
        private const string keyName = "Jwt:Key";
        private readonly SymmetricSecurityKey _key;

        public SymmetricJWTGenerator(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[keyName]));
        }

        public string GenerateToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                new System.Security.Claims.Claim("displayName", user.DisplayName)

            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UserManagementAPI",
                audience: "UserManagementClient",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
