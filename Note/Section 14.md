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

2. FE
