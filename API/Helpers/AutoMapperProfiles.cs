using Api;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    // Cấu hình automapper
    public AutoMapperProfiles()
    {
        /*
            ForMember: Cung cấp quy tắc ánh xạ tùy chỉnh cho một thuộc tính cụ thể.
            d.PhotoUrl là thuộc tính trong đối tượng đích mà ta muốn ánh xạ giá trị.
            o là cấu hình cho thuộc tính hiện tại (PhotoUrl).
            MapFrom chỉ định rằng giá trị của PhotoUrl trong MemberDto sẽ được lấy từ một thuộc tính hoặc biểu thức cụ thể trong AppUser.
            s đại diện cho đối tượng nguồn (AppUser).
            Lấy giá trị của thuộc tính Url từ đối tượng ảnh chính (IsMain).
            Dấu ! là null-forgiving operator, đảm bảo rằng FirstOrDefault sẽ không trả về null.
        */
        CreateMap<AppUser, MemberDto>()
        .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
        .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
    }
}