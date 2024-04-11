using Core.Dtos.Auth;
using Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService(IConfiguration configuration) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        public TokenDto GenerateJwtToken(string userName)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"] ?? "";
            var expireInHours = _configuration["Jwt:ExpireInHours"] ?? "2";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("User", userName)
            };
            DateTime expires = DateTime.Now.AddHours(Convert.ToInt32(expireInHours));
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: expires,
                signingCredentials: credentials,
                claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return new TokenDto { AccessToken = stringToken, ExpiresIn = expires.Ticks, TokenType = "Bearer" };
        }

    }
}