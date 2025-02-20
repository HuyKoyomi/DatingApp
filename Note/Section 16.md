# **Section 11 summary** - Identity and role managenment

1. Common info

- **_Microsoft.AspNetCore.Identity_** '

  - là một thư viện trong ASP.NET Core dùng để quản lý người dùng, vai trò (roles) và xác thực (authentication) trong ứng dụng web
  - Các thành phần chính trong Identity

    - User & Role Management

      - IdentityUser: Lớp đại diện cho người dùng (có sẵn các thuộc tính như UserName, Email, PasswordHash...)
      - IdentityRole: Đại diện cho một vai trò (Admin, User,...)
      - UserManager<TUser>: Dùng để thao tác với người dùng (tạo, cập nhật, xác thực...).
      - RoleManager<TRole>: Dùng để quản lý vai trò.

    - Authentication & Authorization
      - SignInManager<TUser>: Xác thực đăng nhập (cookie, JWT, 2FA).
      - Authorization: Kết hợp với Policy-based Authorization để kiểm soát quyền truy cập.

2. BE

- 197. Setting up the entities

  - sử dụng **_Microsoft.AspNetCore.Identity_** kế thừa bởi entity AppUser

  - Tạo file entity AppRole, AppUserRole và setup AppUser
  - Sửa lại các lỗi đỏ

- 198. Configuring the DbContext

  - thêm thư viện: **_Microsoft.AspNetCore.Identity.EntityFrameworkCore_**
  - sửa file _DataContext.cs_
  - Thiết lập quan hệ User (AppUser), Role (AppRole) và UserRole (AppUserRole) trong Entity Framework Core bằng Fluent API.

- 199. Configuring the startup class

  - cấu hình Identity

- 200. Refactoring and adding a new migration

3. FE
