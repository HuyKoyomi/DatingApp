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
        if (await UserExists(registerDto.UserName))
        {
            return BadRequest("UserName is taken");
        }

        using var hmac = new HMACSHA512(); // Lớp dùng để tạo Hash (mã băm) từ mật khẩu, đảm bảo an toàn cho thông tin người dùng + Tự động tạo một Key được sử dụng làm PasswordSalt

        var user = mapper.Map<AppUser>(registerDto);

        context.Users.Add(user); // thêm đối tượng user vào bảng Users trong cơ sở dữ liệu.
        await context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu một cách bất đồng bộ.
        return new UserDto
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user),
            KnowAs = user.KnownAs,
            Gender = user.Gender,
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
        .Include(p => p.Photos)
        .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null || user.UserName == null)
        {
            return BadRequest("Invalid username");
        }

        return new UserDto
        {
            UserName = user.UserName,
            KnowAs = user.KnownAs,
            Gender = user.Gender,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.NormalizedUserName == username.ToLower());
    }
}
