using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TickCrossLib.Services
{
    public static class JwtService
    {
        public static string Generate(Models.User user)
        {
            Claim[] claims = new Claim[1] { new Claim("userId", user.Id.ToString()) };


            DotNetEnv.Env.Load();
            string key = Environment.GetEnvironmentVariable("SecretKey");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(100));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

    }
}
