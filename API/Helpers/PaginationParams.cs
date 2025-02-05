namespace API.Helpers;

public class PagionationParams
{
    private const int MaxPageSize = 50; // Giới hạn tối đa số lượng phần tử trên mỗi trang là 50.
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10; // _pageSize là một trường private, lưu trữ số lượng phần tử trên mỗi trang.

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}