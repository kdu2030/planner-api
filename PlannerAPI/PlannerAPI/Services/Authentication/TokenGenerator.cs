using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PlannerAPI.Models;

namespace PlannerAPI.Services.Authentication {
    public class TokenGenerator {
        private IConfiguration _configuration;
        private SymmetricSecurityKey _key;
        private JwtSecurityTokenHandler _jwtHandler;

        public TokenGenerator(IConfiguration configuration) {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        public string Generate(PlannerUser user) {
            SigningCredentials credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = { new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Email, user.Email) };
            JwtSecurityToken token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, expires: DateTime.Now.AddDays(15), signingCredentials: credentials);
            token.Header.Add("kid", _configuration["JWT:Key"]);
            return _jwtHandler.WriteToken(token);
        }

        public string Validate(string tokenStr) {
            string key = _configuration["JWT:key"];
            TokenValidationParameters validationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
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
