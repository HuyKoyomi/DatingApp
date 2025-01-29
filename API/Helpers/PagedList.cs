using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

// Lớp PagedList<T> là một lớp generic kế thừa từ List<T>
public class PagedList<T> : List<T>
{

    // Constructor này có nhiệm vụ tạo một danh sách phân trang từ dữ liệu đầu vào.
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Math.Ceiling() để làm tròn lên
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items); // Thêm danh sách items vào danh sách PagedList<T>.

    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}