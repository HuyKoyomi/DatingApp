# **Section 14 summary** - Adding the likes feature

1. BE

Lession 167

- Thêm một bảng phụ trong mối quan hệ many to many
  - Thêm model UserLike
  - Sửa lại AppUser.cs
  - Cập nhật trong DBContext
    - Thêm DbSet
    - sử dụng OnModelCreating để cấu hình quan hệ
    - chạy lênh _dotnet ef migrations add UserLikesAdded_ để update db

Lession 168 + 169

- Thêm file ILikesRepository
- Thêm file LikesService
- Cấu hình scrop

Lession 170
- controller

2. FE

Lession 171
  - ng g s _services/likes --skip-tests
  - file
  - hàm **computed()** 
    + giúp tạo giá trị phụ thuộc vào các Signals khác mà không cần gọi thủ công
    + Tự động theo dõi: Khi bất kỳ Signal nào trong computed() thay đổi, nó sẽ tự động cập nhật.
    + Chỉ tính toán khi cần thiết: Không chạy lại nếu không có sự thay đổi.

  - **OnDestroy** là một lifecycle hook được sử dụng để dọn dẹp (cleanup) tài nguyên khi một component hoặc directive bị hủy

  - Khi nào ngOnDestroy() chạy?
    + Khi component bị loại bỏ khỏi DOM (ví dụ: chuyển trang, *ngIf=false, router-outlet).    
    + Khi directive bị xóa.