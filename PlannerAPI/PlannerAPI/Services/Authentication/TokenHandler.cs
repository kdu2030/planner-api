using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PlannerAPI.Models;

namespace PlannerAPI.Services.Authentication {
    public class TokenHandler {
        private IConfiguration _configuration;
        private SymmetricSecurityKey _key;
        private JwtSecurityTokenHandler _jwtHandler;

        public TokenHandler(IConfiguration configuration) {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        public string Generate(PlannerUser user) {
            SigningCredentials credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = { new Claim(ClaimTypes.NameIdentifier, user.UserName), new Claim(ClaimTypes.Email, user.Email) };
            JwtSecurityToken token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, expires: DateTime.Now.AddDays(15), signingCredentials: credentials);
            return _jwtHandler.WriteToken(token);
        }

        public string Validate(string tokenStr) {
            string key = _configuration["JWT:issuer"];
            TokenValidationParameters validationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = true,
                ValidateAudience = false
            };

            try {
                SecurityToken token;
                _jwtHandler.ValidateToken(tokenStr, validationParameters, out token);
                JwtSecurityToken jwtToken = (JwtSecurityToken) token;
                return jwtToken.Claims.First().Value;
            } catch {
                return "";
            }
        }

    }
}
