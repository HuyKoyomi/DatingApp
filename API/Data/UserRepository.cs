using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
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
    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        // Tạo truy vấn từ bảng Users
        var query = context.Users.AsQueryable(); // Chuyển DbSet<User> thành IQueryable<User>. => IQueryable hỗ trợ truy vấn LINQ động và trì hoãn thực thi (deferred execution).
        query = query.Where(x => x.Username != userParams.CurrentUserName);

        // Thêm điều kiện lọc vào query để chỉ lấy những user có giới tính giống
        if (userParams.Gender != null)
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }

        // Thêm điều kiện lọc vào query theo tuổi
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created), // th "created"
            _ => query.OrderByDescending(x => x.LastActive) // th default
        };

        // Áp dụng phân trang
        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
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