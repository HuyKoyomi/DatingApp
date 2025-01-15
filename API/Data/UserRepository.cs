using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

// UserRepository: Là lớp chịu trách nhiệm thao tác với dữ liệu người dùng trong cơ sở dữ liệu.
// lớp DbContext được sử dụng để làm việc với Entity Framework, giúp truy cập và quản lý cơ sở dữ liệu.
public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
        .Where(x => x.Username == username).ProjectTo<MemberDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        return await context.Users.ProjectTo<MemberDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.Username == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users.Include(x => x.Photos).ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        // Đánh dấu trạng thái của thực thể user là Modified.
        // Điều này giúp Entity Framework biết rằng thực thể này cần được cập nhật vào cơ sở dữ liệu trong lần gọi SaveChangesAsync.
        context.Entry(user).State = EntityState.Modified;
    }
}