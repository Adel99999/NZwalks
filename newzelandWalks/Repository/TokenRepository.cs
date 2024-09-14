using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using newzelandWalks.Models.Domain;
using newzelandWalks.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace newzelandWalks.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenRepository(IConfiguration config,IHttpContextAccessor contextAccessor)
        {
            _config = config;
            _contextAccessor = contextAccessor;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create Claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"], _config["Jwt:Audience"]
                , claims, expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            _contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}
