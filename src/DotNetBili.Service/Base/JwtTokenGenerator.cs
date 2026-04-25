using DotNetBili.Common.Core;
using DotNetBili.Common.Option;
using DotNetBili.IService.Base;
using DotNetBili.Model.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetBili.Service.Base
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(UserInfo user)
        {
            var tokenHandle = new JwtSecurityTokenHandler();

            var jwtOptions = App.GetOptions<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim("UserName", user.NickName),
                new Claim("UserId",user.UserId.ToString()),
            };

            var tokenDescript = new SecurityTokenDescriptor
            {
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescript);

            return tokenHandle.WriteToken(token);
        }

        public string GenerateToken(string userName)
        {
            var tokenHandle = new JwtSecurityTokenHandler();

            var jwtOptions = App.GetOptions<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim("UserName", userName),
            };

            var tokenDescript = new SecurityTokenDescriptor
            {
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescript);

            return tokenHandle.WriteToken(token);
        }
    }
}
