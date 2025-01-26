# **Section 12 summary** - Reactive forms

1. BE

- Adding a photo service
  - Tao file IPhotoService
  - Thêm thư viện **CloudinaryDotNet**
  - Tạo PhotoService
  - Thêm UserController

2. FE

- sử dụng **ReactiveFormsModule**
  - FormGroup
  - FormControl
- validation
  - JsonPipe
  - Validators

3. Các khái niệm

   - **AbstractControl** à lớp trừu tượng trong Angular Forms được sử dụng làm nền tảng cho các lớp điều khiển biểu mẫu khác như:

     - _FormControl_ (đại diện cho một trường input đơn lẻ).
     - _FormGroup_ (đại diện cho một nhóm các control).
     - _FormArray_ (đại diện cho một mảng các control).

   - Học validation trong form
   - Tạo 1 folder **form/text-input** để chứa các hàm text-input dùng chung
   - sử dụng implements _ControlValueAccessor_
     - _ControlValueAccessor_ trong Angular là một interface tạo các component input tùy chỉnh hoạt động như một form control tiêu chuẩn trong _Reactive Forms_ hoặc _Template-driven Forms_.
