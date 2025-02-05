namespace API.Helpers;

// sử dụng để lưu trữ và kiểm soát tham số phân trang (pagination parameters) khi truy vấn danh sách người dùng (User)
public class UserParams: PagionationParams
{
    public string? Gender { get; set; }
    public string? CurrentUserName { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
}