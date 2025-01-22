using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API;

public class PhotoService : IPhotoService
{

    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(config.Value.CloudName, config.Value.Apikey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }
    // IFormFile file: Đối tượng đại diện cho file tải lên từ request HTTP.

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadRes = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream(); // Mở luồng đọc file để chuẩn bị gửi lên Cloudinary.
            var uploadParams = new ImageUploadParams // Dùng using var để tự động giải phóng tài nguyên khi không cần nữa.
            {
                File = new FileDescription(file.FileName, stream), // FileDescription: Chứa tên file và dữ liệu stream để gửi lên Cloudinary.
                Transformation = new Transformation() // Transformation - chuyển đổi ảnh - chiều cao, đồ dài, Cắt ảnh tập trung vào khuôn mặt
                    .Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "angular17-dotnet8"
            };
            uploadRes = await _cloudinary.UploadAsync(uploadParams); // Ảnh được lưu trong thư mục angular17-dotnet8 trên Cloudinary.
        }
        return uploadRes;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
}