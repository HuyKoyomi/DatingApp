namespace API.Helpers;

// sử dụng để lưu trữ và kiểm soát tham số phân trang (pagination parameters) khi truy vấn danh sách người dùng (User)
public class UserParams
{
    private const int MaxPageSize = 50; // Giới hạn tối đa số lượng phần tử trên mỗi trang là 50.
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10; // _pageSize là một trường private, lưu trữ số lượng phần tử trên mỗi trang.

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? Gender { get; set; }
    public string? CurrentUserName { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
}