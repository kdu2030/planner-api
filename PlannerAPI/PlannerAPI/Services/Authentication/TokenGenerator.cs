using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlannerAPI.Services.Authentication {
    public class TokenGenerator {
        private IConfiguration _configuration;
        private SymmetricSecurityKey _key;

        public TokenGenerator(IConfiguration configuration) {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        }

        public string Generate(IdentityUser user) {
            SigningCredentials credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = { new Claim(ClaimTypes.NameIdentifier, user.UserName) };
            JwtSecurityToken token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
