using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
        {
            return BadRequest("Username is taken");
        }

        using var hmac = new HMACSHA512(); // Lớp dùng để tạo Hash (mã băm) từ mật khẩu, đảm bảo an toàn cho thông tin người dùng + Tự động tạo một Key được sử dụng làm PasswordSalt

        var user = mapper.Map<AppUser>(registerDto);
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)); // Mã hóa mật khẩu người dùng bằng HMACSHA512.
        user.PasswordSalt = hmac.Key; // Lưu trữ Key từ HMACSHA512 để giải mã mật khẩu sau này.

        context.Users.Add(user); // thêm đối tượng user vào bảng Users trong cơ sở dữ liệu.
        await context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu một cách bất đồng bộ.
        return new UserDto
        {
            Username = user.Username,
            Token = tokenService.CreateToken(user),
            KnowAs = user.KnownAs,
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
        .Include(p => p.Photos)
        .FirstOrDefaultAsync(x => x.Username == loginDto.Username.ToLower());

        if (user == null)
        {
            return BadRequest("Invalid username");
        }
        using var hmac = new HMACSHA512(user.PasswordSalt); //  Khởi tạo HMACSHA512 với PasswordSalt
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // Tính toán PasswordHash từ mật khẩu nhập vào => Tạo mã băm (hash) từ mật khẩu và PasswordSalt

        for (int i = 0; i < computeHash.Length; i++) // So sánh PasswordHash
        {
            if (computeHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        return new UserDto
        {
            Username = user.Username,
            KnowAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
    }
}
