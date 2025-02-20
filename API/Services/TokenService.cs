using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Humanizer;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            // Lấy TokenKey từ cấu hình
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appseting");
            if (tokenKey.Length < 64) throw new Exception("Your tokenkey need to be longer");

            // Tạo khóa mã hóa đối xứng
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // => Chuyển đổi TokenKey thành mảng byte sử dụng mã hóa UTF-8.

            // Claim: Đại diện cho thông tin người dùng
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new(ClaimTypes.Name, appUser.UserName)

            };

            // Tạo thông tin ký sốs
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Mô tả cấu trúc token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}