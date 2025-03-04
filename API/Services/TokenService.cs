using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
    {
        public async Task<string> CreateToken(AppUser appUser)
        {
            // Lấy TokenKey từ cấu hình
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appseting");
            if (tokenKey.Length < 64) throw new Exception("Your tokenkey need to be longer");

            // Tạo khóa mã hóa đối xứng
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // => Chuyển đổi TokenKey thành mảng byte sử dụng mã hóa UTF-8.

            if (appUser.UserName == null) throw new Exception("No username for user");

            // Claim: Đại diện cho thông tin người dùng
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new(ClaimTypes.Name, appUser.UserName)

            };

            // Taọ role
            var roles = await userManager.GetRolesAsync(appUser);
            // Add roles to token
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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