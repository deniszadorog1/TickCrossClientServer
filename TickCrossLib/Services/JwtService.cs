using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

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
