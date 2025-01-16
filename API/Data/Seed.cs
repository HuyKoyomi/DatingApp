using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        // Đọc thông tinn từ file json
        if (await context.Users.AnyAsync()) return;
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null)
        {
            return;
        }

        // Tạo pasword cho các user
        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();
            user.Username = user.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
            user.PasswordSalt = hmac.Key;
            context.Users.Add(user); // thêm đối tượng user vào bảng Users trong cơ sở dữ liệu.
        }
        await context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu một cách bất đồng bộ.
    }

    public static async Task SeedInterest(DataContext context)
    {
        // Kiểm tra nếu bảng Interests đã có dữ liệu thì thoát
        if (await context.Interest.AnyAsync()) return;

        // Tạo danh sách các sở thích mẫu
        var interests = new List<Interest>
        {
            new Interest { Name = "Thể thao", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Interest { Name = "Âm nhạc", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Interest { Name = "Du lịch", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Interest { Name = "Đọc sách", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Interest { Name = "Nấu ăn", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };
        // Thêm các sở thích vào bảng Interest
        context.Interest.AddRange(interests);
        await context.SaveChangesAsync();
    }
}